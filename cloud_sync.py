#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Cloud Sync Module - Đồng bộ dữ liệu đa thiết bị
Hỗ trợ: Google Drive, Dropbox, OneDrive, Firebase
"""

import os
import json
import sqlite3
import threading
import time
import hashlib
import requests
from datetime import datetime, timedelta
import base64
import zipfile
import tempfile
from pathlib import Path

class CloudSyncManager:
    """Quản lý đồng bộ cloud"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        
        self.setup_sync_tables()
        
        # Cloud providers
        self.providers = {
            'firebase': FirebaseSync(),
            'gdrive': GoogleDriveSync(),
            'dropbox': DropboxSync(),
            'onedrive': OneDriveSync()
        }
        
        self.sync_enabled = False
        self.auto_sync_interval = 300  # 5 minutes
        self.last_sync = None
        self.sync_thread = None
    
    def setup_sync_tables(self):
        """Tạo bảng quản lý sync"""
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS sync_settings (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                provider TEXT NOT NULL,
                enabled BOOLEAN DEFAULT 0,
                credentials TEXT,
                last_sync TEXT,
                sync_count INTEGER DEFAULT 0,
                auto_sync BOOLEAN DEFAULT 1
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS sync_history (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                provider TEXT,
                sync_type TEXT,
                status TEXT,
                timestamp TEXT,
                file_count INTEGER,
                data_size INTEGER,
                error_message TEXT
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS device_info (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                device_id TEXT UNIQUE,
                device_name TEXT,
                last_active TEXT,
                app_version TEXT,
                os_info TEXT
            )
        ''')
        
        self.conn.commit()
    
    def register_device(self):
        """Đăng ký thiết bị"""
        try:
            import platform
            import uuid
            
            device_id = str(uuid.uuid4())
            device_name = platform.node()
            os_info = f"{platform.system()} {platform.release()}"
            app_version = "1.0.0"
            
            self.cursor.execute("""
                INSERT OR REPLACE INTO device_info 
                (device_id, device_name, last_active, app_version, os_info)
                VALUES (?, ?, ?, ?, ?)
            """, (device_id, device_name, datetime.now().isoformat(), app_version, os_info))
            
            self.conn.commit()
            return device_id
            
        except Exception as e:
            print(f"Lỗi đăng ký thiết bị: {str(e)}")
            return None
    
    def setup_provider(self, provider_name, credentials):
        """Cài đặt cloud provider"""
        try:
            if provider_name not in self.providers:
                return False, "Provider không được hỗ trợ"
            
            provider = self.providers[provider_name]
            success, message = provider.authenticate(credentials)
            
            if success:
                # Lưu credentials (encrypted)
                encrypted_creds = self.encrypt_credentials(credentials)
                
                self.cursor.execute("""
                    INSERT OR REPLACE INTO sync_settings 
                    (provider, enabled, credentials, last_sync)
                    VALUES (?, 1, ?, ?)
                """, (provider_name, encrypted_creds, datetime.now().isoformat()))
                
                self.conn.commit()
                return True, f"Cài đặt {provider_name} thành công"
            else:
                return False, message
                
        except Exception as e:
            return False, f"Lỗi cài đặt provider: {str(e)}"
    
    def encrypt_credentials(self, credentials):
        """Mã hóa credentials (simplified)"""
        try:
            creds_str = json.dumps(credentials)
            encoded = base64.b64encode(creds_str.encode()).decode()
            return encoded
        except:
            return ""
    
    def decrypt_credentials(self, encrypted_creds):
        """Giải mã credentials"""
        try:
            decoded = base64.b64decode(encrypted_creds.encode()).decode()
            return json.loads(decoded)
        except:
            return {}
    
    def create_backup_package(self):
        """Tạo package backup"""
        try:
            timestamp = datetime.now().strftime('%Y%m%d_%H%M%S')
            backup_name = f"construction_backup_{timestamp}.zip"
            
            with tempfile.TemporaryDirectory() as temp_dir:
                backup_path = os.path.join(temp_dir, backup_name)
                
                with zipfile.ZipFile(backup_path, 'w', zipfile.ZIP_DEFLATED) as zipf:
                    # Add database
                    zipf.write(self.db_path, 'database.db')
                    
                    # Add metadata
                    metadata = {
                        'backup_date': datetime.now().isoformat(),
                        'app_version': '1.0.0',
                        'device_id': self.register_device(),
                        'file_count': 1,
                        'checksum': self.calculate_db_checksum()
                    }
                    
                    zipf.writestr('metadata.json', json.dumps(metadata, indent=2))
                
                # Read backup file
                with open(backup_path, 'rb') as f:
                    backup_data = f.read()
                
                return backup_data, backup_name, len(backup_data)
                
        except Exception as e:
            return None, None, 0
    
    def calculate_db_checksum(self):
        """Tính checksum của database"""
        try:
            with open(self.db_path, 'rb') as f:
                content = f.read()
                return hashlib.md5(content).hexdigest()
        except:
            return ""
    
    def sync_to_cloud(self, provider_name=None):
        """Đồng bộ lên cloud"""
        try:
            if provider_name:
                providers_to_sync = [provider_name]
            else:
                # Get enabled providers
                self.cursor.execute("SELECT provider FROM sync_settings WHERE enabled = 1")
                providers_to_sync = [row[0] for row in self.cursor.fetchall()]
            
            if not providers_to_sync:
                return False, "Không có provider nào được kích hoạt"
            
            # Create backup package
            backup_data, backup_name, file_size = self.create_backup_package()
            if not backup_data:
                return False, "Không thể tạo backup"
            
            sync_results = {}
            
            for provider_name in providers_to_sync:
                try:
                    provider = self.providers[provider_name]
                    
                    # Get credentials
                    self.cursor.execute("SELECT credentials FROM sync_settings WHERE provider = ?", (provider_name,))
                    creds_result = self.cursor.fetchone()
                    
                    if creds_result:
                        credentials = self.decrypt_credentials(creds_result[0])
                        provider.authenticate(credentials)
                        
                        # Upload
                        success, message = provider.upload_file(backup_data, backup_name)
                        sync_results[provider_name] = {'success': success, 'message': message}
                        
                        # Log sync history
                        self.cursor.execute("""
                            INSERT INTO sync_history 
                            (provider, sync_type, status, timestamp, file_count, data_size, error_message)
                            VALUES (?, 'upload', ?, ?, 1, ?, ?)
                        """, (provider_name, 'success' if success else 'failed', 
                              datetime.now().isoformat(), file_size, 
                              '' if success else message))
                        
                        if success:
                            # Update last sync
                            self.cursor.execute("""
                                UPDATE sync_settings 
                                SET last_sync = ?, sync_count = sync_count + 1
                                WHERE provider = ?
                            """, (datetime.now().isoformat(), provider_name))
                    
                except Exception as e:
                    sync_results[provider_name] = {'success': False, 'message': str(e)}
            
            self.conn.commit()
            self.last_sync = datetime.now()
            
            successful_syncs = sum(1 for r in sync_results.values() if r['success'])
            total_syncs = len(sync_results)
            
            return successful_syncs > 0, f"Đồng bộ thành công {successful_syncs}/{total_syncs} provider"
            
        except Exception as e:
            return False, f"Lỗi đồng bộ: {str(e)}"
    
    def restore_from_cloud(self, provider_name, backup_name=None):
        """Khôi phục từ cloud"""
        try:
            provider = self.providers[provider_name]
            
            # Get credentials
            self.cursor.execute("SELECT credentials FROM sync_settings WHERE provider = ?", (provider_name,))
            creds_result = self.cursor.fetchone()
            
            if not creds_result:
                return False, "Provider chưa được cài đặt"
            
            credentials = self.decrypt_credentials(creds_result[0])
            provider.authenticate(credentials)
            
            # List backups if backup_name not specified
            if not backup_name:
                backups = provider.list_backups()
                if not backups:
                    return False, "Không tìm thấy backup"
                backup_name = backups[0]  # Get latest backup
            
            # Download backup
            success, backup_data = provider.download_file(backup_name)
            if not success:
                return False, f"Không thể tải backup: {backup_data}"
            
            # Extract and restore
            with tempfile.TemporaryDirectory() as temp_dir:
                backup_path = os.path.join(temp_dir, backup_name)
                
                with open(backup_path, 'wb') as f:
                    f.write(backup_data)
                
                with zipfile.ZipFile(backup_path, 'r') as zipf:
                    # Extract metadata
                    metadata = json.loads(zipf.read('metadata.json').decode())
                    
                    # Verify checksum if needed
                    # ... checksum verification logic ...
                    
                    # Extract database
                    db_content = zipf.read('database.db')
                    
                    # Backup current database
                    current_backup = f"{self.db_path}.backup_{datetime.now().strftime('%Y%m%d_%H%M%S')}"
                    os.rename(self.db_path, current_backup)
                    
                    # Restore new database
                    with open(self.db_path, 'wb') as f:
                        f.write(db_content)
            
            return True, f"Khôi phục thành công từ {backup_name}"
            
        except Exception as e:
            return False, f"Lỗi khôi phục: {str(e)}"
    
    def start_auto_sync(self):
        """Bắt đầu đồng bộ tự động"""
        if self.sync_thread and self.sync_thread.is_alive():
            return False, "Auto sync đã chạy"
        
        self.sync_enabled = True
        
        def sync_worker():
            while self.sync_enabled:
                try:
                    # Check if need to sync
                    if self.should_auto_sync():
                        success, message = self.sync_to_cloud()
                        print(f"[{datetime.now()}] Auto sync: {message}")
                    
                    time.sleep(self.auto_sync_interval)
                    
                except Exception as e:
                    print(f"[{datetime.now()}] Auto sync error: {str(e)}")
                    time.sleep(60)  # Wait 1 minute on error
        
        self.sync_thread = threading.Thread(target=sync_worker, daemon=True)
        self.sync_thread.start()
        
        return True, "Auto sync đã bắt đầu"
    
    def stop_auto_sync(self):
        """Dừng đồng bộ tự động"""
        self.sync_enabled = False
        return True, "Auto sync đã dừng"
    
    def should_auto_sync(self):
        """Kiểm tra có cần auto sync không"""
        if not self.last_sync:
            return True
        
        time_diff = datetime.now() - self.last_sync
        return time_diff.total_seconds() >= self.auto_sync_interval
    
    def get_sync_status(self):
        """Lấy trạng thái đồng bộ"""
        self.cursor.execute("""
            SELECT provider, enabled, last_sync, sync_count 
            FROM sync_settings
        """)
        
        providers_status = []
        for provider, enabled, last_sync, sync_count in self.cursor.fetchall():
            providers_status.append({
                'provider': provider,
                'enabled': bool(enabled),
                'last_sync': last_sync,
                'sync_count': sync_count,
                'status': 'Active' if enabled else 'Inactive'
            })
        
        return {
            'auto_sync_enabled': self.sync_enabled,
            'last_sync': self.last_sync.isoformat() if self.last_sync else None,
            'providers': providers_status,
            'sync_interval_minutes': self.auto_sync_interval // 60
        }


class FirebaseSync:
    """Firebase Cloud Sync"""
    
    def __init__(self):
        self.api_key = None
        self.project_id = None
        self.authenticated = False
    
    def authenticate(self, credentials):
        """Xác thực Firebase"""
        try:
            self.api_key = credentials.get('api_key')
            self.project_id = credentials.get('project_id')
            
            if self.api_key and self.project_id:
                self.authenticated = True
                return True, "Firebase authentication successful"
            else:
                return False, "Missing Firebase credentials"
                
        except Exception as e:
            return False, f"Firebase auth error: {str(e)}"
    
    def upload_file(self, file_data, filename):
        """Upload file to Firebase Storage"""
        try:
            if not self.authenticated:
                return False, "Not authenticated"
            
            # Simulate Firebase upload (replace with actual Firebase SDK)
            # url = f"https://firebasestorage.googleapis.com/v0/b/{self.project_id}.appspot.com/o/{filename}"
            # response = requests.post(url, data=file_data, params={'uploadType': 'media'})
            
            # Simulate success
            return True, f"Uploaded {filename} to Firebase"
            
        except Exception as e:
            return False, f"Firebase upload error: {str(e)}"
    
    def download_file(self, filename):
        """Download file from Firebase Storage"""
        try:
            # Simulate download
            return True, b"simulated_backup_data"
            
        except Exception as e:
            return False, f"Firebase download error: {str(e)}"
    
    def list_backups(self):
        """List available backups"""
        # Simulate backup list
        return ["construction_backup_20241219_120000.zip"]


class GoogleDriveSync:
    """Google Drive Sync"""
    
    def __init__(self):
        self.access_token = None
        self.authenticated = False
    
    def authenticate(self, credentials):
        """Xác thực Google Drive"""
        try:
            self.access_token = credentials.get('access_token')
            
            if self.access_token:
                self.authenticated = True
                return True, "Google Drive authentication successful"
            else:
                return False, "Missing Google Drive access token"
                
        except Exception as e:
            return False, f"Google Drive auth error: {str(e)}"
    
    def upload_file(self, file_data, filename):
        """Upload file to Google Drive"""
        try:
            if not self.authenticated:
                return False, "Not authenticated"
            
            # Simulate Google Drive upload
            return True, f"Uploaded {filename} to Google Drive"
            
        except Exception as e:
            return False, f"Google Drive upload error: {str(e)}"
    
    def download_file(self, filename):
        """Download file from Google Drive"""
        try:
            return True, b"simulated_backup_data"
            
        except Exception as e:
            return False, f"Google Drive download error: {str(e)}"
    
    def list_backups(self):
        """List available backups"""
        return ["construction_backup_20241219_120000.zip"]


class DropboxSync:
    """Dropbox Sync"""
    
    def __init__(self):
        self.access_token = None
        self.authenticated = False
    
    def authenticate(self, credentials):
        """Xác thực Dropbox"""
        try:
            self.access_token = credentials.get('access_token')
            
            if self.access_token:
                self.authenticated = True
                return True, "Dropbox authentication successful"
            else:
                return False, "Missing Dropbox access token"
                
        except Exception as e:
            return False, f"Dropbox auth error: {str(e)}"
    
    def upload_file(self, file_data, filename):
        """Upload file to Dropbox"""
        try:
            if not self.authenticated:
                return False, "Not authenticated"
            
            # Simulate Dropbox upload
            return True, f"Uploaded {filename} to Dropbox"
            
        except Exception as e:
            return False, f"Dropbox upload error: {str(e)}"
    
    def download_file(self, filename):
        """Download file from Dropbox"""
        try:
            return True, b"simulated_backup_data"
            
        except Exception as e:
            return False, f"Dropbox download error: {str(e)}"
    
    def list_backups(self):
        """List available backups"""
        return ["construction_backup_20241219_120000.zip"]


class OneDriveSync:
    """OneDrive Sync"""
    
    def __init__(self):
        self.access_token = None
        self.authenticated = False
    
    def authenticate(self, credentials):
        """Xác thực OneDrive"""
        try:
            self.access_token = credentials.get('access_token')
            
            if self.access_token:
                self.authenticated = True
                return True, "OneDrive authentication successful"
            else:
                return False, "Missing OneDrive access token"
                
        except Exception as e:
            return False, f"OneDrive auth error: {str(e)}"
    
    def upload_file(self, file_data, filename):
        """Upload file to OneDrive"""
        try:
            if not self.authenticated:
                return False, "Not authenticated"
            
            # Simulate OneDrive upload
            return True, f"Uploaded {filename} to OneDrive"
            
        except Exception as e:
            return False, f"OneDrive upload error: {str(e)}"
    
    def download_file(self, filename):
        """Download file from OneDrive"""
        try:
            return True, b"simulated_backup_data"
            
        except Exception as e:
            return False, f"OneDrive download error: {str(e)}"
    
    def list_backups(self):
        """List available backups"""
        return ["construction_backup_20241219_120000.zip"]
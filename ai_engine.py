#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
AI Engine cho phần mềm dự toán xây dựng
Tính năng: Dự báo giá, gợi ý vật liệu thay thế, phân tích chi phí tối ưu
"""

import numpy as np
import pandas as pd
from sklearn.linear_model import LinearRegression
from sklearn.ensemble import RandomForestRegressor
from sklearn.preprocessing import StandardScaler
import sqlite3
from datetime import datetime, timedelta
import json
import threading
import warnings
warnings.filterwarnings('ignore')

class PricePredictionAI:
    """AI dự báo giá vật liệu và định mức"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        
        # Models
        self.price_model = RandomForestRegressor(n_estimators=100, random_state=42)
        self.trend_model = LinearRegression()
        self.scaler = StandardScaler()
        
        # Training data cache
        self.model_trained = False
        self.last_training = None
        
        self.setup_ai_tables()
    
    def setup_ai_tables(self):
        """Tạo bảng lưu kết quả AI"""
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS ai_predictions (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                norm_code TEXT,
                prediction_type TEXT,
                current_price REAL,
                predicted_price REAL,
                confidence REAL,
                prediction_date TEXT,
                target_date TEXT,
                factors TEXT
            )
        ''')
        
        self.cursor.execute('''
            CREATE TABLE IF NOT EXISTS material_alternatives (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                original_code TEXT,
                alternative_code TEXT,
                similarity_score REAL,
                cost_difference REAL,
                performance_rating REAL,
                recommendation_reason TEXT
            )
        ''')
        self.conn.commit()
    
    def prepare_training_data(self):
        """Chuẩn bị dữ liệu training"""
        # Lấy lịch sử giá
        self.cursor.execute("""
            SELECT norm_code, price, date, source 
            FROM price_history 
            ORDER BY date ASC
        """)
        
        price_history = pd.DataFrame(self.cursor.fetchall(), 
                                   columns=['norm_code', 'price', 'date', 'source'])
        
        if price_history.empty:
            return None, None
        
        # Convert date
        price_history['date'] = pd.to_datetime(price_history['date'])
        price_history['days_from_start'] = (price_history['date'] - price_history['date'].min()).dt.days
        price_history['month'] = price_history['date'].dt.month
        price_history['quarter'] = price_history['date'].dt.quarter
        price_history['year'] = price_history['date'].dt.year
        
        # Tính các chỉ số kỹ thuật
        for code in price_history['norm_code'].unique():
            code_data = price_history[price_history['norm_code'] == code].copy()
            
            if len(code_data) > 3:
                # Moving averages
                code_data['ma_7'] = code_data['price'].rolling(window=min(7, len(code_data))).mean()
                code_data['ma_30'] = code_data['price'].rolling(window=min(30, len(code_data))).mean()
                
                # Price change
                code_data['price_change'] = code_data['price'].pct_change()
                code_data['price_volatility'] = code_data['price_change'].rolling(window=min(10, len(code_data))).std()
                
                price_history.loc[price_history['norm_code'] == code, 'ma_7'] = code_data['ma_7']
                price_history.loc[price_history['norm_code'] == code, 'ma_30'] = code_data['ma_30']
                price_history.loc[price_history['norm_code'] == code, 'price_change'] = code_data['price_change']
                price_history.loc[price_history['norm_code'] == code, 'price_volatility'] = code_data['price_volatility']
        
        # Features và targets
        feature_columns = ['days_from_start', 'month', 'quarter', 'year', 'ma_7', 'ma_30', 'price_volatility']
        
        # Encode categorical variables
        price_history['source_encoded'] = pd.Categorical(price_history['source']).codes
        price_history['norm_encoded'] = pd.Categorical(price_history['norm_code']).codes
        
        feature_columns.extend(['source_encoded', 'norm_encoded'])
        
        # Remove NaN values
        clean_data = price_history.dropna(subset=feature_columns + ['price'])
        
        if clean_data.empty:
            return None, None
        
        X = clean_data[feature_columns].values
        y = clean_data['price'].values
        
        return X, y
    
    def train_models(self):
        """Training AI models"""
        try:
            X, y = self.prepare_training_data()
            
            if X is None or len(X) < 10:
                return False, "Không đủ dữ liệu để training AI"
            
            # Scale features
            X_scaled = self.scaler.fit_transform(X)
            
            # Train price prediction model
            self.price_model.fit(X_scaled, y)
            
            # Train trend model (simplified)
            trend_X = X[:, 0].reshape(-1, 1)  # days_from_start
            self.trend_model.fit(trend_X, y)
            
            self.model_trained = True
            self.last_training = datetime.now()
            
            return True, "Training AI thành công"
            
        except Exception as e:
            return False, f"Lỗi training AI: {str(e)}"
    
    def predict_price(self, norm_code, days_ahead=30):
        """Dự báo giá định mức"""
        if not self.model_trained:
            success, message = self.train_models()
            if not success:
                return None, message
        
        try:
            # Lấy dữ liệu gần nhất của norm_code
            self.cursor.execute("""
                SELECT price, date FROM price_history 
                WHERE norm_code = ? 
                ORDER BY date DESC LIMIT 10
            """, (norm_code,))
            
            recent_data = self.cursor.fetchall()
            if not recent_data:
                return None, "Không có dữ liệu lịch sử giá"
            
            current_price = recent_data[0][0]
            
            # Tạo features cho prediction
            latest_date = datetime.fromisoformat(recent_data[0][1])
            target_date = latest_date + timedelta(days=days_ahead)
            
            # Giả lập features (cần cải thiện với dữ liệu thực)
            features = np.array([[
                days_ahead,  # days_from_start (relative)
                target_date.month,  # month
                (target_date.month - 1) // 3 + 1,  # quarter
                target_date.year,  # year
                current_price,  # ma_7 (approximation)
                current_price,  # ma_30 (approximation)
                0.1,  # price_volatility (approximation)
                0,  # source_encoded
                hash(norm_code) % 100  # norm_encoded (approximation)
            ]])
            
            # Scale features
            features_scaled = self.scaler.transform(features)
            
            # Predict
            predicted_price = self.price_model.predict(features_scaled)[0]
            
            # Calculate confidence (simplified)
            confidence = min(0.95, max(0.60, 1 - abs(predicted_price - current_price) / current_price))
            
            # Lưu kết quả
            self.cursor.execute("""
                INSERT INTO ai_predictions 
                (norm_code, prediction_type, current_price, predicted_price, confidence, prediction_date, target_date)
                VALUES (?, 'price_forecast', ?, ?, ?, ?, ?)
            """, (norm_code, current_price, predicted_price, confidence, 
                  datetime.now().isoformat(), target_date.isoformat()))
            self.conn.commit()
            
            return {
                'norm_code': norm_code,
                'current_price': current_price,
                'predicted_price': predicted_price,
                'price_change': predicted_price - current_price,
                'price_change_percent': ((predicted_price - current_price) / current_price) * 100,
                'confidence': confidence,
                'days_ahead': days_ahead,
                'target_date': target_date.strftime('%d/%m/%Y')
            }, "Dự báo thành công"
            
        except Exception as e:
            return None, f"Lỗi dự báo giá: {str(e)}"
    
    def analyze_price_trend(self, norm_code, period_days=365):
        """Phân tích xu hướng giá"""
        try:
            start_date = (datetime.now() - timedelta(days=period_days)).isoformat()
            
            self.cursor.execute("""
                SELECT price, date FROM price_history
                WHERE norm_code = ? AND date >= ?
                ORDER BY date ASC
            """, (norm_code, start_date))
            
            data = self.cursor.fetchall()
            if len(data) < 3:
                return None, "Không đủ dữ liệu để phân tích xu hướng"
            
            prices = [row[0] for row in data]
            dates = [datetime.fromisoformat(row[1]) for row in data]
            
            # Tính toán các chỉ số
            price_changes = np.diff(prices)
            avg_change = np.mean(price_changes)
            volatility = np.std(price_changes)
            
            # Xác định xu hướng
            if avg_change > volatility * 0.1:
                trend = "Tăng"
                trend_strength = min(100, abs(avg_change) / volatility * 20)
            elif avg_change < -volatility * 0.1:
                trend = "Giảm"
                trend_strength = min(100, abs(avg_change) / volatility * 20)
            else:
                trend = "Ổn định"
                trend_strength = max(0, 100 - volatility / np.mean(prices) * 100)
            
            # Dự báo xu hướng
            future_prediction, _ = self.predict_price(norm_code, 30)
            
            return {
                'norm_code': norm_code,
                'period_days': period_days,
                'trend': trend,
                'trend_strength': trend_strength,
                'average_change': avg_change,
                'volatility': volatility,
                'min_price': min(prices),
                'max_price': max(prices),
                'current_price': prices[-1],
                'price_range': max(prices) - min(prices),
                'future_prediction': future_prediction
            }, "Phân tích xu hướng thành công"
            
        except Exception as e:
            return None, f"Lỗi phân tích xu hướng: {str(e)}"


class MaterialOptimizer:
    """AI tối ưu hóa vật liệu và gợi ý thay thế"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
        
        # Bảng tương đương vật liệu
        self.material_equivalents = {
            'beton': {
                'C15': ['C20', 'C25'],
                'C20': ['C15', 'C25', 'C30'],
                'C25': ['C20', 'C30']
            },
            'thep': {
                'CB240-T': ['CB300-V', 'CB400-V'],
                'CB300-V': ['CB240-T', 'CB400-V'],
                'CB400-V': ['CB300-V', 'CB500-V']
            },
            'gach': {
                'gach_ong': ['gach_block', 'gach_bong'],
                'gach_block': ['gach_ong', 'gach_bong']
            }
        }
    
    def find_material_alternatives(self, norm_code, max_alternatives=5):
        """Tìm vật liệu thay thế"""
        try:
            # Lấy thông tin định mức gốc
            self.cursor.execute("""
                SELECT name, unit, total_cost, category FROM norms WHERE code = ?
            """, (norm_code,))
            
            original = self.cursor.fetchone()
            if not original:
                return None, "Không tìm thấy định mức"
            
            original_name, original_unit, original_cost, original_category = original
            
            # Tìm các định mức tương tự
            self.cursor.execute("""
                SELECT code, name, unit, total_cost, category 
                FROM norms 
                WHERE category = ? AND code != ? AND unit = ?
                ORDER BY ABS(total_cost - ?) ASC
                LIMIT ?
            """, (original_category, norm_code, original_unit, original_cost, max_alternatives * 2))
            
            candidates = self.cursor.fetchall()
            
            alternatives = []
            for code, name, unit, cost, category in candidates:
                # Tính similarity score
                similarity = self.calculate_similarity(original_name, name)
                cost_difference = cost - original_cost
                cost_difference_percent = (cost_difference / original_cost) * 100
                
                # Performance rating (simplified)
                performance_rating = self.estimate_performance_rating(original_name, name, cost_difference_percent)
                
                # Recommendation reason
                reason = self.generate_recommendation_reason(cost_difference_percent, similarity, performance_rating)
                
                alternatives.append({
                    'code': code,
                    'name': name,
                    'unit': unit,
                    'cost': cost,
                    'cost_difference': cost_difference,
                    'cost_difference_percent': cost_difference_percent,
                    'similarity_score': similarity,
                    'performance_rating': performance_rating,
                    'recommendation_reason': reason
                })
                
                # Lưu vào database
                self.cursor.execute("""
                    INSERT OR REPLACE INTO material_alternatives
                    (original_code, alternative_code, similarity_score, cost_difference, performance_rating, recommendation_reason)
                    VALUES (?, ?, ?, ?, ?, ?)
                """, (norm_code, code, similarity, cost_difference, performance_rating, reason))
            
            self.conn.commit()
            
            # Sắp xếp theo điểm tổng hợp
            alternatives.sort(key=lambda x: x['similarity_score'] * 0.4 + x['performance_rating'] * 0.6, reverse=True)
            
            return alternatives[:max_alternatives], "Tìm thấy vật liệu thay thế"
            
        except Exception as e:
            return None, f"Lỗi tìm vật liệu thay thế: {str(e)}"
    
    def calculate_similarity(self, name1, name2):
        """Tính độ tương đồng giữa 2 tên vật liệu"""
        name1_lower = name1.lower()
        name2_lower = name2.lower()
        
        # Simple similarity based on common words
        words1 = set(name1_lower.split())
        words2 = set(name2_lower.split())
        
        intersection = words1.intersection(words2)
        union = words1.union(words2)
        
        if len(union) == 0:
            return 0
        
        return len(intersection) / len(union)
    
    def estimate_performance_rating(self, original_name, alternative_name, cost_difference_percent):
        """Ước tính đánh giá hiệu suất"""
        base_rating = 0.7  # Base rating
        
        # Adjust based on cost difference
        if cost_difference_percent < -20:  # Rẻ hơn 20%
            base_rating += 0.2
        elif cost_difference_percent > 20:  # Đắt hơn 20%
            base_rating += 0.1
        
        # Adjust based on material grade (simplified)
        if 'C25' in alternative_name and 'C15' in original_name:
            base_rating += 0.1  # Higher grade concrete
        elif 'C15' in alternative_name and 'C25' in original_name:
            base_rating -= 0.1  # Lower grade concrete
        
        return min(1.0, max(0.1, base_rating))
    
    def generate_recommendation_reason(self, cost_diff_percent, similarity, performance):
        """Tạo lý do gợi ý"""
        reasons = []
        
        if cost_diff_percent < -10:
            reasons.append(f"Tiết kiệm {abs(cost_diff_percent):.1f}% chi phí")
        elif cost_diff_percent > 10:
            reasons.append(f"Đắt hơn {cost_diff_percent:.1f}% nhưng chất lượng tốt hơn")
        
        if similarity > 0.7:
            reasons.append("Tính năng tương đương")
        elif similarity > 0.5:
            reasons.append("Tính năng tương tự")
        
        if performance > 0.8:
            reasons.append("Hiệu suất cao")
        elif performance > 0.6:
            reasons.append("Hiệu suất tốt")
        
        return "; ".join(reasons) if reasons else "Vật liệu thay thế phù hợp"
    
    def optimize_project_cost(self, project_items):
        """Tối ưu chi phí toàn bộ dự án"""
        try:
            optimization_results = []
            total_current_cost = 0
            total_optimized_cost = 0
            
            for item in project_items:
                norm_code = item.get('norm_code')
                quantity = item.get('quantity', 0)
                current_price = item.get('unit_price', 0)
                
                total_current_cost += quantity * current_price
                
                # Tìm vật liệu thay thế
                alternatives, _ = self.find_material_alternatives(norm_code, 3)
                
                best_alternative = None
                if alternatives:
                    # Chọn alternative tốt nhất (cân bằng giữa giá và chất lượng)
                    for alt in alternatives:
                        score = alt['performance_rating'] * 0.6 - (alt['cost_difference_percent'] / 100) * 0.4
                        alt['optimization_score'] = score
                    
                    best_alternative = max(alternatives, key=lambda x: x['optimization_score'])
                
                item_result = {
                    'original': {
                        'code': norm_code,
                        'price': current_price,
                        'quantity': quantity,
                        'total': quantity * current_price
                    },
                    'best_alternative': best_alternative,
                    'potential_savings': 0,
                    'recommendation': 'Giữ nguyên'
                }
                
                if best_alternative and best_alternative['cost_difference'] < 0:
                    savings = quantity * abs(best_alternative['cost_difference'])
                    item_result['potential_savings'] = savings
                    item_result['recommendation'] = f"Thay thế để tiết kiệm {savings:,.0f} VNĐ"
                    total_optimized_cost += quantity * best_alternative['cost']
                else:
                    total_optimized_cost += quantity * current_price
                
                optimization_results.append(item_result)
            
            total_savings = total_current_cost - total_optimized_cost
            savings_percent = (total_savings / total_current_cost) * 100 if total_current_cost > 0 else 0
            
            return {
                'items': optimization_results,
                'summary': {
                    'current_total_cost': total_current_cost,
                    'optimized_total_cost': total_optimized_cost,
                    'total_savings': total_savings,
                    'savings_percent': savings_percent,
                    'optimization_count': len([r for r in optimization_results if r['potential_savings'] > 0])
                }
            }, "Tối ưu chi phí thành công"
            
        except Exception as e:
            return None, f"Lỗi tối ưu chi phí: {str(e)}"


class MarketAnalysisAI:
    """AI phân tích thị trường xây dựng"""
    
    def __init__(self, db_path='construction_estimate.db'):
        self.db_path = db_path
        self.conn = sqlite3.connect(db_path, check_same_thread=False)
        self.cursor = self.conn.cursor()
    
    def analyze_market_conditions(self):
        """Phân tích tình hình thị trường"""
        try:
            # Lấy dữ liệu giá gần đây
            recent_date = (datetime.now() - timedelta(days=90)).isoformat()
            
            self.cursor.execute("""
                SELECT norm_code, AVG(price) as avg_price, COUNT(*) as data_points
                FROM price_history 
                WHERE date >= ?
                GROUP BY norm_code
                HAVING COUNT(*) >= 3
            """, (recent_date,))
            
            recent_prices = self.cursor.fetchall()
            
            if not recent_prices:
                return None, "Không đủ dữ liệu thị trường"
            
            # Phân tích theo category
            category_analysis = {}
            
            for norm_code, avg_price, data_points in recent_prices:
                # Lấy category
                self.cursor.execute("SELECT category FROM norms WHERE code = ?", (norm_code,))
                category_result = self.cursor.fetchone()
                category = category_result[0] if category_result else 'Khác'
                
                if category not in category_analysis:
                    category_analysis[category] = {
                        'items': [],
                        'avg_price': 0,
                        'total_items': 0
                    }
                
                category_analysis[category]['items'].append({
                    'code': norm_code,
                    'avg_price': avg_price,
                    'data_points': data_points
                })
                category_analysis[category]['total_items'] += 1
            
            # Tính average cho mỗi category
            for category in category_analysis:
                prices = [item['avg_price'] for item in category_analysis[category]['items']]
                category_analysis[category]['avg_price'] = np.mean(prices)
                category_analysis[category]['price_std'] = np.std(prices)
            
            # Market insights
            insights = self.generate_market_insights(category_analysis)
            
            return {
                'analysis_date': datetime.now().strftime('%d/%m/%Y'),
                'period': '90 ngày gần đây',
                'categories': category_analysis,
                'insights': insights,
                'total_categories': len(category_analysis),
                'total_items_analyzed': sum(cat['total_items'] for cat in category_analysis.values())
            }, "Phân tích thị trường thành công"
            
        except Exception as e:
            return None, f"Lỗi phân tích thị trường: {str(e)}"
    
    def generate_market_insights(self, category_analysis):
        """Tạo insights thị trường"""
        insights = []
        
        # Tìm category có giá cao nhất
        highest_price_category = max(category_analysis.keys(), 
                                   key=lambda k: category_analysis[k]['avg_price'])
        insights.append(f"Nhóm vật liệu đắt nhất: {highest_price_category}")
        
        # Tìm category có biến động cao nhất
        highest_volatility_category = max(category_analysis.keys(),
                                        key=lambda k: category_analysis[k].get('price_std', 0))
        insights.append(f"Nhóm biến động giá cao nhất: {highest_volatility_category}")
        
        # Gợi ý chiến lược
        insights.append("Nên theo dõi sát giá nhóm vật liệu có biến động cao")
        insights.append("Cân nhắc mua trước đối với vật liệu có xu hướng tăng giá")
        
        return insights
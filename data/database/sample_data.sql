-- Sample data for Vietnamese Construction Estimator
-- Basic materials commonly used in Vietnam construction

-- Insert basic materials
INSERT INTO Materials (Code, Name, Description, Unit, Category, CurrentPrice, Origin, Quality) VALUES
-- Cement and Concrete
('CEM001', 'Xi măng PC40', 'Xi măng poocland thường', 'tấn', 3, 2800000, 'Việt Nam', 'Tiêu chuẩn'),
('CEM002', 'Xi măng PC50', 'Xi măng poocland cao cấp', 'tấn', 3, 3200000, 'Việt Nam', 'Cao cấp'),
('CON001', 'Bê tông B15', 'Bê tông mác 150', 'm3', 1, 1850000, 'Việt Nam', 'Tiêu chuẩn'),
('CON002', 'Bê tông B20', 'Bê tông mác 200', 'm3', 1, 2150000, 'Việt Nam', 'Tiêu chuẩn'),
('CON003', 'Bê tông B25', 'Bê tông mác 250', 'm3', 1, 2450000, 'Việt Nam', 'Tiêu chuẩn'),

-- Steel
('STE001', 'Thép CB240-T', 'Thép tròn trơn phi 6', 'tấn', 2, 18500000, 'Việt Nam', 'CB240-T'),
('STE002', 'Thép CB300-V', 'Thép tròn gân phi 10', 'tấn', 2, 19200000, 'Việt Nam', 'CB300-V'),
('STE003', 'Thép CB400-V', 'Thép tròn gân phi 12', 'tấn', 2, 19800000, 'Việt Nam', 'CB400-V'),
('STE004', 'Thép CB500-V', 'Thép tròn gân phi 16', 'tấn', 2, 20500000, 'Việt Nam', 'CB500-V'),

-- Sand and Stone
('SAN001', 'Cát vàng', 'Cát vàng sông', 'm3', 4, 450000, 'Việt Nam', 'Loại 1'),
('SAN002', 'Cát xây', 'Cát xây dựng', 'm3', 4, 420000, 'Việt Nam', 'Loại 1'),
('STO001', 'Đá 1x2', 'Đá dăm cỡ 1x2 cm', 'm3', 5, 480000, 'Việt Nam', 'Loại 1'),
('STO002', 'Đá 2x4', 'Đá dăm cỡ 2x4 cm', 'm3', 5, 510000, 'Việt Nam', 'Loại 1'),

-- Bricks
('BRI001', 'Gạch đất sét nung', 'Gạch đỏ 220x105x60', 'viên', 6, 850, 'Việt Nam', 'Loại A'),
('BRI002', 'Gạch block', 'Gạch không nung 200x100x60', 'viên', 6, 1200, 'Việt Nam', 'M75'),
('BRI003', 'Gạch 6 lỗ', 'Gạch đất sét nung 6 lỗ', 'viên', 6, 2800, 'Việt Nam', 'Loại A'),

-- Wood
('WOO001', 'Gỗ thông', 'Gỗ thông xẻ 5x10', 'm3', 8, 8500000, 'Việt Nam', 'Loại II'),
('WOO002', 'Gỗ sao', 'Gỗ sao xẻ 5x20', 'm3', 8, 12000000, 'Việt Nam', 'Loại I'),
('WOO003', 'Gỗ dẻ gai', 'Gỗ dẻ gai xẻ 10x10', 'm3', 8, 15000000, 'Việt Nam', 'Loại I');

-- Insert basic labor types
INSERT INTO Labor (Code, Name, Description, Type, Unit, CurrentCost, SkillLevel, Region) VALUES
-- Unskilled labor
('LAB001', 'Công phổ thông', 'Lao động phổ thông', 1, 'công', 350000, 1, 'Hà Nội'),
('LAB002', 'Công phụ hồ', 'Công nhân phụ hồ', 1, 'công', 380000, 2, 'Hà Nội'),

-- Skilled labor
('LAB003', 'Thợ hồ bậc 3', 'Thợ xây bậc 3/7', 2, 'công', 520000, 3, 'Hà Nội'),
('LAB004', 'Thợ hồ bậc 4', 'Thợ xây bậc 4/7', 2, 'công', 580000, 4, 'Hà Nội'),
('LAB005', 'Thợ hồ bậc 5', 'Thợ xây bậc 5/7', 2, 'công', 650000, 5, 'Hà Nội'),

-- Specialized labor
('LAB006', 'Thợ sắt bậc 4', 'Thợ gia công thép bậc 4/7', 3, 'công', 620000, 4, 'Hà Nội'),
('LAB007', 'Thợ sắt bậc 5', 'Thợ gia công thép bậc 5/7', 3, 'công', 720000, 5, 'Hà Nội'),
('LAB008', 'Thợ mộc bậc 4', 'Thợ mộc bậc 4/7', 3, 'công', 580000, 4, 'Hà Nội'),
('LAB009', 'Thợ điện bậc 5', 'Thợ điện bậc 5/7', 3, 'công', 680000, 5, 'Hà Nội'),
('LAB010', 'Thợ cơ khí bậc 6', 'Thợ cơ khí bậc 6/7', 3, 'công', 780000, 6, 'Hà Nội');

-- Insert basic equipment
INSERT INTO Equipment (Code, Name, Description, Category, Unit, CurrentCost, Manufacturer, RequiresOperator) VALUES
-- Excavation equipment
('EQU001', 'Máy xúc PC120', 'Máy xúc bánh xích 1.2m3', 1, 'ca', 3200000, 'Komatsu', 1),
('EQU002', 'Máy xúc PC200', 'Máy xúc bánh xích 2.0m3', 1, 'ca', 4500000, 'Komatsu', 1),

-- Transportation
('EQU003', 'Xe tải 5 tấn', 'Xe tải chở vật liệu 5 tấn', 4, 'ca', 1800000, 'Hyundai', 1),
('EQU004', 'Xe tải 15 tấn', 'Xe tải chở vật liệu 15 tấn', 4, 'ca', 2500000, 'Hyundai', 1),

-- Concrete equipment
('EQU005', 'Máy trộn bê tông 250L', 'Máy trộn bê tông di động', 3, 'ca', 250000, 'Việt Nam', 1),
('EQU006', 'Máy bơm bê tông', 'Máy bơm bê tông di động', 3, 'ca', 2800000, 'Putzmeister', 1),

-- Compaction
('EQU007', 'Máy đầm cóc', 'Máy đầm đất nhỏ', 5, 'ca', 180000, 'Mikasa', 1),
('EQU008', 'Máy đầm bánh xích', 'Máy đầm bánh xích 14 tấn', 5, 'ca', 2200000, 'Dynapac', 1);

-- Insert basic construction standards
INSERT INTO Standards (Code, Name, Description, Unit, Category, Version, IssuedBy) VALUES
-- Concrete work standards
('STD001', 'Đổ bê tông móng băng', 'Thi công đổ bê tông móng băng', 'm3', 2, '1.0', 'Bộ Xây dựng'),
('STD002', 'Đổ bê tông cột', 'Thi công đổ bê tông cột', 'm3', 3, '1.0', 'Bộ Xây dựng'),
('STD003', 'Đổ bê tông dầm', 'Thi công đổ bê tông dầm', 'm3', 3, '1.0', 'Bộ Xây dựng'),
('STD004', 'Đổ bê tông sàn', 'Thi công đổ bê tông sàn', 'm3', 3, '1.0', 'Bộ Xây dựng'),

-- Masonry work standards
('STD005', 'Xây tường gạch nung', 'Xây tường gạch nung dày 220mm', 'm2', 4, '1.0', 'Bộ Xây dựng'),
('STD006', 'Xây tường gạch block', 'Xây tường gạch block dày 200mm', 'm2', 4, '1.0', 'Bộ Xây dựng');

-- Insert standard material relationships (example for concrete foundation)
INSERT INTO StandardMaterials (StandardId, MaterialId, QuantityPerUnit, WasteFactor) VALUES
-- For concrete foundation (STD001) - 1m3 concrete
(1, 3, 1.0, 3.0),  -- B20 concrete: 1.0 m3 per m3 work
(1, 12, 0.05, 5.0), -- Sand: 0.05 m3 per m3 work
(1, 13, 0.08, 5.0); -- Stone 1x2: 0.08 m3 per m3 work

-- Insert standard labor relationships
INSERT INTO StandardLabor (StandardId, LaborId, QuantityPerUnit) VALUES
-- For concrete foundation (STD001)
(1, 1, 0.15),  -- Unskilled labor: 0.15 days per m3
(1, 3, 0.08),  -- Mason level 3: 0.08 days per m3
(1, 6, 0.05);  -- Steel worker level 4: 0.05 days per m3

-- Insert standard equipment relationships
INSERT INTO StandardEquipment (StandardId, EquipmentId, QuantityPerUnit) VALUES
-- For concrete foundation (STD001)
(1, 5, 0.2),   -- Concrete mixer: 0.2 shifts per m3
(1, 7, 0.1);   -- Compactor: 0.1 shifts per m3
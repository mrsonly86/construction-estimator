-- Construction Estimator Database Schema
-- SQLite database for Vietnamese Construction Estimation Software
-- Created: 2024

-- Enable foreign key constraints
PRAGMA foreign_keys = ON;

-- Projects table
CREATE TABLE IF NOT EXISTS Projects (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 1000),
    Location TEXT NOT NULL CHECK(length(Location) <= 100),
    Client TEXT NOT NULL CHECK(length(Client) <= 100),
    Contractor TEXT NOT NULL CHECK(length(Contractor) <= 100),
    CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModifiedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedBy TEXT CHECK(length(CreatedBy) <= 50),
    LastModifiedBy TEXT CHECK(length(LastModifiedBy) <= 50),
    Status INTEGER NOT NULL DEFAULT 0,
    TotalCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    VatRate DECIMAL(5,2) NOT NULL DEFAULT 10,
    ProfitMargin DECIMAL(5,2) NOT NULL DEFAULT 15,
    GeneralCostsRate DECIMAL(5,2) NOT NULL DEFAULT 8,
    Settings TEXT CHECK(length(Settings) <= 2000)
);

-- Materials table
CREATE TABLE IF NOT EXISTS Materials (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE CHECK(length(Code) <= 50),
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 500),
    Unit TEXT NOT NULL CHECK(length(Unit) <= 20),
    Category INTEGER NOT NULL,
    Supplier TEXT CHECK(length(Supplier) <= 100),
    CurrentPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    Origin TEXT CHECK(length(Origin) <= 100),
    Quality TEXT CHECK(length(Quality) <= 50),
    Specifications TEXT CHECK(length(Specifications) <= 1000),
    CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastUpdatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsActive INTEGER NOT NULL DEFAULT 1
);

-- Material Prices table
CREATE TABLE IF NOT EXISTS MaterialPrices (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    MaterialId INTEGER NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Region TEXT CHECK(length(Region) <= 100),
    Supplier TEXT CHECK(length(Supplier) <= 100),
    EffectiveDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Notes TEXT CHECK(length(Notes) <= 200),
    IsActive INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id) ON DELETE CASCADE
);

-- Labor table
CREATE TABLE IF NOT EXISTS Labor (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE CHECK(length(Code) <= 50),
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 500),
    Type INTEGER NOT NULL,
    Unit TEXT NOT NULL CHECK(length(Unit) <= 20),
    CurrentCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    SkillLevel INTEGER NOT NULL,
    Region TEXT CHECK(length(Region) <= 100),
    CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastUpdatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsActive INTEGER NOT NULL DEFAULT 1
);

-- Labor Costs table
CREATE TABLE IF NOT EXISTS LaborCosts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    LaborId INTEGER NOT NULL,
    Cost DECIMAL(18,2) NOT NULL,
    Region TEXT CHECK(length(Region) <= 100),
    EffectiveDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Notes TEXT CHECK(length(Notes) <= 200),
    IsActive INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (LaborId) REFERENCES Labor(Id) ON DELETE CASCADE
);

-- Equipment table
CREATE TABLE IF NOT EXISTS Equipment (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE CHECK(length(Code) <= 50),
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 500),
    Category INTEGER NOT NULL,
    Unit TEXT NOT NULL CHECK(length(Unit) <= 20),
    CurrentCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    Manufacturer TEXT CHECK(length(Manufacturer) <= 100),
    Model TEXT CHECK(length(Model) <= 50),
    Specifications TEXT CHECK(length(Specifications) <= 1000),
    FuelConsumption DECIMAL(10,2),
    RequiresOperator INTEGER NOT NULL DEFAULT 1,
    CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastUpdatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsActive INTEGER NOT NULL DEFAULT 1
);

-- Equipment Costs table
CREATE TABLE IF NOT EXISTS EquipmentCosts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EquipmentId INTEGER NOT NULL,
    Cost DECIMAL(18,2) NOT NULL,
    Region TEXT CHECK(length(Region) <= 100),
    Supplier TEXT CHECK(length(Supplier) <= 100),
    EffectiveDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Notes TEXT CHECK(length(Notes) <= 200),
    IsActive INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id) ON DELETE CASCADE
);

-- Standards table
CREATE TABLE IF NOT EXISTS Standards (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Code TEXT NOT NULL UNIQUE CHECK(length(Code) <= 50),
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 1000),
    Unit TEXT NOT NULL CHECK(length(Unit) <= 20),
    Category INTEGER NOT NULL,
    Formula TEXT CHECK(length(Formula) <= 500),
    Version TEXT NOT NULL DEFAULT '1.0' CHECK(length(Version) <= 20),
    IssuedBy TEXT CHECK(length(IssuedBy) <= 100),
    IssuedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    EffectiveDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    ExpiryDate TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1,
    Notes TEXT CHECK(length(Notes) <= 1000)
);

-- Estimate Items table
CREATE TABLE IF NOT EXISTS EstimateItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ProjectId INTEGER NOT NULL,
    Code TEXT NOT NULL CHECK(length(Code) <= 50),
    Name TEXT NOT NULL CHECK(length(Name) <= 200),
    Description TEXT CHECK(length(Description) <= 1000),
    Unit TEXT NOT NULL CHECK(length(Unit) <= 20),
    Quantity DECIMAL(18,4) NOT NULL DEFAULT 0,
    UnitPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    Category TEXT CHECK(length(Category) <= 100),
    ParentId INTEGER,
    SortOrder INTEGER NOT NULL DEFAULT 0,
    StandardId INTEGER,
    Notes TEXT CHECK(length(Notes) <= 1000),
    CreatedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    LastModifiedDate TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP,
    IsActive INTEGER NOT NULL DEFAULT 1,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (ParentId) REFERENCES EstimateItems(Id) ON DELETE CASCADE,
    FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE SET NULL
);

-- Junction tables for many-to-many relationships

-- Estimate Item Materials
CREATE TABLE IF NOT EXISTS EstimateItemMaterials (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EstimateItemId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    TotalQuantity DECIMAL(18,4) NOT NULL DEFAULT 0,
    UnitCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    WasteFactor DECIMAL(5,2) NOT NULL DEFAULT 5,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (EstimateItemId) REFERENCES EstimateItems(Id) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id) ON DELETE CASCADE
);

-- Estimate Item Labor
CREATE TABLE IF NOT EXISTS EstimateItemLabor (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EstimateItemId INTEGER NOT NULL,
    LaborId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    TotalQuantity DECIMAL(18,4) NOT NULL DEFAULT 0,
    UnitCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (EstimateItemId) REFERENCES EstimateItems(Id) ON DELETE CASCADE,
    FOREIGN KEY (LaborId) REFERENCES Labor(Id) ON DELETE CASCADE
);

-- Estimate Item Equipment
CREATE TABLE IF NOT EXISTS EstimateItemEquipment (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    EstimateItemId INTEGER NOT NULL,
    EquipmentId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    TotalQuantity DECIMAL(18,4) NOT NULL DEFAULT 0,
    UnitCost DECIMAL(18,2) NOT NULL DEFAULT 0,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (EstimateItemId) REFERENCES EstimateItems(Id) ON DELETE CASCADE,
    FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id) ON DELETE CASCADE
);

-- Standard Materials
CREATE TABLE IF NOT EXISTS StandardMaterials (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StandardId INTEGER NOT NULL,
    MaterialId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    WasteFactor DECIMAL(5,2) NOT NULL DEFAULT 5,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE CASCADE,
    FOREIGN KEY (MaterialId) REFERENCES Materials(Id) ON DELETE CASCADE
);

-- Standard Labor
CREATE TABLE IF NOT EXISTS StandardLabor (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StandardId INTEGER NOT NULL,
    LaborId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE CASCADE,
    FOREIGN KEY (LaborId) REFERENCES Labor(Id) ON DELETE CASCADE
);

-- Standard Equipment
CREATE TABLE IF NOT EXISTS StandardEquipment (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StandardId INTEGER NOT NULL,
    EquipmentId INTEGER NOT NULL,
    QuantityPerUnit DECIMAL(18,4) NOT NULL DEFAULT 0,
    Notes TEXT CHECK(length(Notes) <= 200),
    FOREIGN KEY (StandardId) REFERENCES Standards(Id) ON DELETE CASCADE,
    FOREIGN KEY (EquipmentId) REFERENCES Equipment(Id) ON DELETE CASCADE
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_materials_code ON Materials(Code);
CREATE INDEX IF NOT EXISTS idx_materials_category ON Materials(Category);
CREATE INDEX IF NOT EXISTS idx_labor_code ON Labor(Code);
CREATE INDEX IF NOT EXISTS idx_equipment_code ON Equipment(Code);
CREATE INDEX IF NOT EXISTS idx_standards_code ON Standards(Code);
CREATE INDEX IF NOT EXISTS idx_estimateitems_project ON EstimateItems(ProjectId);
CREATE INDEX IF NOT EXISTS idx_estimateitems_parent ON EstimateItems(ParentId);
CREATE INDEX IF NOT EXISTS idx_materialprices_material ON MaterialPrices(MaterialId);
CREATE INDEX IF NOT EXISTS idx_laborcosts_labor ON LaborCosts(LaborId);
CREATE INDEX IF NOT EXISTS idx_equipmentcosts_equipment ON EquipmentCosts(EquipmentId);
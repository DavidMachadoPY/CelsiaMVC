-- Active: 1723578079629@@bxw4ik7zjybf5cdakluz-mysql.services.clever-cloud.com@3306@bxw4ik7zjybf5cdakluz

CREATE TABLE Users (
    Id VARCHAR(50) PRIMARY KEY,
    Name VARCHAR(255),
    Address TEXT,
    Password VARCHAR(255),
    Phone VARCHAR(50),
    Email VARCHAR(255)
    -- Un usuario puede tener muchas transacciones, pero cada transacción está asociada con un solo usuario.
    -- Esta es una relación de uno a muchos (one-to-many).
);

CREATE TABLE Platforms (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255)
    -- Una plataforma de pago puede ser utilizada en muchas transacciones, pero cada transacción está asociada con una sola plataforma.
    -- Esta es una relación de uno a muchos (one-to-many).
);

CREATE TABLE Transactions (
    Id VARCHAR(50) PRIMARY KEY,
    DateTime DATETIME,
    Amount DECIMAL(18, 2),
    Status VARCHAR(50),
    Type VARCHAR(50),
    UserId VARCHAR(50),
    PlatformId INT,
    
    -- Relación de uno a muchos: Un usuario puede tener muchas transacciones.
    -- Este campo establece la relación entre una transacción y un usuario.
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    
    -- Relación de uno a muchos: Una plataforma puede ser utilizada en muchas transacciones.
    -- Este campo establece la relación entre una transacción y una plataforma.
    FOREIGN KEY (PlatformId) REFERENCES Platforms(Id)
);

CREATE TABLE Invoices (
    Number VARCHAR(50) PRIMARY KEY,
    TransactionId VARCHAR(50),
    Period VARCHAR(50),
    BilledAmount DECIMAL(18, 2),
    PaidAmount DECIMAL(18, 2),
    
    -- Relación de uno a uno o uno a muchos: Cada transacción puede tener una o más facturas asociadas.
    -- Este campo establece la relación entre una factura y una transacción.
    FOREIGN KEY (TransactionId) REFERENCES Transactions(Id)
    -- Aunque normalmente una factura se asocia a una transacción única, si permitimos que una transacción tenga múltiples facturas,
    -- esto se convertiría en una relación de uno a muchos.
);

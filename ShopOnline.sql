-- ====== Begin::Level 1 ========
CREATE TABLE Banners(
	BAN_ID			int PRIMARY KEY IDENTITY,
	Title			nvarchar(255) NOT NULL,
	[Image]			nvarchar(255),
	[Url]			varchar(255),
	DisplayOrder	int,
	CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE Settings(
	SET_ID			int PRIMARY KEY IDENTITY,
	[Name]			nvarchar(255) not null,
	[Value]			nvarchar(500) not null,
	CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE Categories (
    CAT_ID			int PRIMARY KEY IDENTITY,
    [Name]			nvarchar(255) NOT NULL,
    DisplayOrder	int,
	CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

-- ====== End::Level 1 ========
-- ====== Begin::Level 2 ========
CREATE TABLE Products (
    PRO_ID			int PRIMARY KEY IDENTITY,
    CAT_ID			int NOT NULL REFERENCES Categories(CAT_ID),
    [Image]			nvarchar(255),
    Title			nvarchar(255) NOT NULL,
    Subtitle		nvarchar(255),
    Price			money,
    Rate			float,
	CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE Carts (
    CAR_ID			int PRIMARY KEY IDENTITY, 
    USE_ID			nvarchar(450) NOT NULL REFERENCES Users(Id),
    Customer		nvarchar(255),
    Phone			nvarchar(255),
    [Address]		nvarchar(255),
    TotalPrice		money,
    PaymentMethod	nvarchar(255),
    IsPaid			nvarchar(255),
	Note			nvarchar(255),
    CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE Orders (
    ORD_ID			int PRIMARY KEY IDENTITY,
    USE_ID			nvarchar(450) NOT NULL REFERENCES Users(id),
    Customer		nvarchar(255),
    Phone			nvarchar(255),
    [Address]		nvarchar(255),
    TotalPrice		money,
    PaymentMethod	nvarchar(255),
    IsPaid			bit DEFAULT(0),
    Note			nvarchar(255),
    CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);
-- ====== End::Level 2 ========
-- ====== Begin::Level 3 ========
CREATE TABLE Reviews (
	REV_ID			int PRIMARY KEY IDENTITY,
    USE_ID			nvarchar(450) NOT NULL REFERENCES Users(Id),
    PRO_ID			int NOT NULL REFERENCES Products(PRO_ID),    
    Rate			float NOT NULL CHECK (Rate >= 0.0 AND Rate <= 5.0), -- Assuming the rating is between 0 and 5
    Comment			nvarchar(255),
	CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE OrderDetails (
    ORDD_ID			int PRIMARY KEY IDENTITY,
    ORD_ID			int NOT NULL REFERENCES Orders(ORD_ID), 
    PRO_ID			int NOT NULL REFERENCES Products(PRO_ID), 
    Quantity		int NOT NULL CHECK (Quantity > 0), 
    Price			money NOT NULL,
    CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);

CREATE TABLE CartDetails (
    CARD_ID			int PRIMARY KEY IDENTITY,
    CAR_ID			int NOT NULL REFERENCES Carts(CAR_ID), 
    PRO_ID			int NOT NULL REFERENCES Products(PRO_ID), 
    Quantity		int NOT NULL CHECK (Quantity > 0), 
    Price			money NOT NULL,
    CreatedDate		datetime NOT NULL DEFAULT(getdate()),
	CreatedBy		nvarchar(255) NOT NULL,
	UpdatedDate		datetime DEFAULT(getdate()),
	UpdatedBy		nvarchar(255)
);
-- ====== End::Level 3 ========



IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [CreateDate] datetime2 NOT NULL,
    [UpdateDate] datetime2 NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [Cupons] (
    [Id] int NOT NULL IDENTITY,
    [Code] nvarchar(max) NULL,
    [DiscountAmount] decimal(18,2) NOT NULL,
    [DiscountType] nvarchar(max) NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [MinimumOrderAmount] decimal(18,2) NOT NULL,
    [UsageCount] int NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Cupons] PRIMARY KEY ([Id])
);

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [UserName] nvarchar(50) NOT NULL,
    [PasswordHash] varbinary(max) NULL,
    [PasswordSalt] varbinary(max) NULL,
    [FullName] nvarchar(100) NULL,
    [Email] nvarchar(max) NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [IsAdmin] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NOT NULL,
    [IsEmailConfirmed] bit NOT NULL,
    [IsPhoneNumberConfirmed] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [CategoryId] int NOT NULL,
    [StockQuantity] int NOT NULL,
    [CreateDate] datetime2 NOT NULL,
    [UpdateDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Carts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [Notifications] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [OrderDate] datetime2 NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [UserId] int NULL,
    [Status] nvarchar(max) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [UserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Wishlists] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Wishlists] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Wishlists_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Favorites] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_Favorites] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Favorites_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Favorites_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductImages] (
    [Id] int NOT NULL IDENTITY,
    [ImageUrl] nvarchar(max) NULL,
    [IsCover] bit NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductRatings] (
    [Id] int NOT NULL IDENTITY,
    [Rating] int NOT NULL,
    [ProductId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_ProductRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductRatings_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductRatings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductReviews] (
    [Id] int NOT NULL IDENTITY,
    [Review] nvarchar(max) NULL,
    [Rating] int NOT NULL,
    [ReviewDate] datetime2 NOT NULL,
    [ProductId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_ProductReviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductReviews_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductReviews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Stocks] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_Stocks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Stocks_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CartItems] (
    [Id] int NOT NULL IDENTITY,
    [CartId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Invoices] (
    [Id] int NOT NULL IDENTITY,
    [InvoiceDate] datetime2 NOT NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [OrderId] int NOT NULL,
    CONSTRAINT [PK_Invoices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Invoices_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Logs] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NULL,
    [LogDate] datetime2 NOT NULL,
    [LogLevel] nvarchar(max) NULL,
    [OrderId] int NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Logs_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]),
    CONSTRAINT [FK_Logs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [OrderDetails] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentDate] datetime2 NOT NULL,
    [PaymentMethod] nvarchar(max) NULL,
    [OrderId] int NOT NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ReturnRequests] (
    [Id] int NOT NULL IDENTITY,
    [Reason] nvarchar(max) NULL,
    [RequestDate] datetime2 NOT NULL,
    [OrderId] int NOT NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_ReturnRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ReturnRequests_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ReturnRequests_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [Shipments] (
    [Id] int NOT NULL IDENTITY,
    [TrackingNumber] nvarchar(max) NULL,
    [Carrier] nvarchar(max) NULL,
    [ShippedDate] datetime2 NOT NULL,
    [DeliveredDate] datetime2 NOT NULL,
    [OrderId] int NOT NULL,
    CONSTRAINT [PK_Shipments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Shipments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [WishlistItems] (
    [Id] int NOT NULL IDENTITY,
    [WishlistId] int NOT NULL,
    [ProductId] int NOT NULL,
    CONSTRAINT [PK_WishlistItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WishlistItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WishlistItems_Wishlists_WishlistId] FOREIGN KEY ([WishlistId]) REFERENCES [Wishlists] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AuctionImages] (
    [Id] int NOT NULL IDENTITY,
    [ImageUrl] nvarchar(max) NULL,
    [IsCover] bit NOT NULL,
    [AuctionId] int NOT NULL,
    CONSTRAINT [PK_AuctionImages] PRIMARY KEY ([Id])
);

CREATE TABLE [Auctions] (
    [Id] int NOT NULL IDENTITY,
    [ProductName] nvarchar(max) NULL,
    [StartingPrice] decimal(18,2) NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [EndTime] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [WinningBidId] int NULL,
    [ProductId] int NULL,
    CONSTRAINT [PK_Auctions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Auctions_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id])
);

CREATE TABLE [Bids] (
    [Id] int NOT NULL IDENTITY,
    [Amount] decimal(18,2) NOT NULL,
    [BidTime] datetime2 NOT NULL,
    [UserId] int NOT NULL,
    [AuctionId] int NOT NULL,
    CONSTRAINT [PK_Bids] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bids_Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [Auctions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Bids_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AuctionImages_AuctionId] ON [AuctionImages] ([AuctionId]);

CREATE INDEX [IX_Auctions_ProductId] ON [Auctions] ([ProductId]);

CREATE INDEX [IX_Auctions_WinningBidId] ON [Auctions] ([WinningBidId]);

CREATE INDEX [IX_Bids_AuctionId] ON [Bids] ([AuctionId]);

CREATE INDEX [IX_Bids_UserId] ON [Bids] ([UserId]);

CREATE INDEX [IX_CartItems_CartId] ON [CartItems] ([CartId]);

CREATE INDEX [IX_CartItems_ProductId] ON [CartItems] ([ProductId]);

CREATE UNIQUE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]) WHERE [UserId] IS NOT NULL;

CREATE INDEX [IX_Favorites_ProductId] ON [Favorites] ([ProductId]);

CREATE INDEX [IX_Favorites_UserId] ON [Favorites] ([UserId]);

CREATE UNIQUE INDEX [IX_Invoices_OrderId] ON [Invoices] ([OrderId]);

CREATE INDEX [IX_Logs_OrderId] ON [Logs] ([OrderId]);

CREATE INDEX [IX_Logs_UserId] ON [Logs] ([UserId]);

CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);

CREATE INDEX [IX_OrderDetails_OrderId] ON [OrderDetails] ([OrderId]);

CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);

CREATE UNIQUE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);

CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);

CREATE INDEX [IX_ProductRatings_ProductId] ON [ProductRatings] ([ProductId]);

CREATE INDEX [IX_ProductRatings_UserId] ON [ProductRatings] ([UserId]);

CREATE INDEX [IX_ProductReviews_ProductId] ON [ProductReviews] ([ProductId]);

CREATE INDEX [IX_ProductReviews_UserId] ON [ProductReviews] ([UserId]);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

CREATE INDEX [IX_ReturnRequests_OrderId] ON [ReturnRequests] ([OrderId]);

CREATE INDEX [IX_ReturnRequests_UserId] ON [ReturnRequests] ([UserId]);

CREATE UNIQUE INDEX [IX_Shipments_OrderId] ON [Shipments] ([OrderId]);

CREATE UNIQUE INDEX [IX_Stocks_ProductId] ON [Stocks] ([ProductId]);

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

CREATE INDEX [IX_WishlistItems_ProductId] ON [WishlistItems] ([ProductId]);

CREATE INDEX [IX_WishlistItems_WishlistId] ON [WishlistItems] ([WishlistId]);

CREATE INDEX [IX_Wishlists_UserId] ON [Wishlists] ([UserId]);

ALTER TABLE [AuctionImages] ADD CONSTRAINT [FK_AuctionImages_Auctions_AuctionId] FOREIGN KEY ([AuctionId]) REFERENCES [Auctions] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Auctions] ADD CONSTRAINT [FK_Auctions_Bids_WinningBidId] FOREIGN KEY ([WinningBidId]) REFERENCES [Bids] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250710144632_Initial', N'9.0.6');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'StockQuantity');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Products] DROP COLUMN [StockQuantity];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250712225225_RemoveStockQuantityFromProduct', N'9.0.6');

ALTER TABLE [Wishlists] ADD [CreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [WishlistItems] ADD [AddedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Orders] ADD [AddressId] int NOT NULL DEFAULT 0;

ALTER TABLE [CartItems] ADD [AddedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [CartItems] ADD [UnitPrice] decimal(18,2) NOT NULL DEFAULT 0.0;

CREATE TABLE [Address] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [FullName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [District] nvarchar(max) NULL,
    [Neighborhood] nvarchar(max) NULL,
    [AddressLine] nvarchar(max) NULL,
    [PostalCode] nvarchar(max) NULL,
    [IsDefault] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Address] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Address_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CustomOrderRequests] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Phone] nvarchar(max) NULL,
    [ProductType] nvarchar(max) NULL,
    [Dimensions] nvarchar(max) NULL,
    [WoodType] nvarchar(max) NULL,
    [Color] nvarchar(max) NULL,
    [AdditionalNote] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_CustomOrderRequests] PRIMARY KEY ([Id])
);

CREATE INDEX [IX_Orders_AddressId] ON [Orders] ([AddressId]);

CREATE INDEX [IX_Address_UserId] ON [Address] ([UserId]);

ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Address_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Address] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250718231548_AddCustomOrderAndAddress', N'9.0.6');

ALTER TABLE [Address] ADD [Title] nvarchar(max) NULL;

ALTER TABLE [Address] ADD [UpdatedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250719214747_UpdateAddressTable', N'9.0.6');

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Cupons]') AND [c].[name] = N'DiscountType');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Cupons] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Cupons] DROP COLUMN [DiscountType];

ALTER TABLE [Orders] ADD [CuponId] int NULL;

ALTER TABLE [Orders] ADD [DiscountAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

CREATE INDEX [IX_Orders_CuponId] ON [Orders] ([CuponId]);

ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Cupons_CuponId] FOREIGN KEY ([CuponId]) REFERENCES [Cupons] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250719220817_UpdateCuponOrderTable', N'9.0.6');

ALTER TABLE [Cupons] ADD [DiscountType] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250720070924_OrderTableUpdate', N'9.0.6');

ALTER TABLE [Products] ADD [DiscountEndDate] datetime2 NULL;

ALTER TABLE [Products] ADD [DiscountStartDate] datetime2 NULL;

ALTER TABLE [Products] ADD [DiscountedPrice] decimal(18,2) NULL;

ALTER TABLE [Carts] ADD [CuponId] int NULL;

ALTER TABLE [Carts] ADD [DiscountAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

CREATE INDEX [IX_Carts_CuponId] ON [Carts] ([CuponId]);

ALTER TABLE [Carts] ADD CONSTRAINT [FK_Carts_Cupons_CuponId] FOREIGN KEY ([CuponId]) REFERENCES [Cupons] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250721164230_AddProductFieldAndCupon', N'9.0.6');

ALTER TABLE [OrderDetails] ADD [DiscountedPrice] decimal(18,2) NULL;

ALTER TABLE [CartItems] ADD [DiscountedPrice] decimal(18,2) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250721221208_AddFieldForOrderDetail', N'9.0.6');

ALTER TABLE [CartItems] ADD [SessionId] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722092738_SessionFieldInCart', N'9.0.6');

ALTER TABLE [Orders] DROP CONSTRAINT [FK_Orders_Address_AddressId];

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'AddressId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Orders] ALTER COLUMN [AddressId] int NULL;

ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Address_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Address] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722113526_AddressIdNullable', N'9.0.6');

ALTER TABLE [Address] DROP CONSTRAINT [FK_Address_Users_UserId];

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Address]') AND [c].[name] = N'UserId');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Address] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Address] ALTER COLUMN [UserId] int NULL;

ALTER TABLE [Address] ADD CONSTRAINT [FK_Address_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722114348_AddressIdNullable2', N'9.0.6');

ALTER TABLE [CartItems] ADD [UserId] int NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250722121239_CartItemUserId', N'9.0.6');

CREATE TABLE [BankAccounts] (
    [Id] int NOT NULL IDENTITY,
    [BankName] nvarchar(max) NULL,
    [AccountNumber] nvarchar(max) NULL,
    [IBAN] nvarchar(max) NULL,
    [AccountHolder] nvarchar(max) NULL,
    CONSTRAINT [PK_BankAccounts] PRIMARY KEY ([Id])
);

CREATE TABLE [PaymentMethods] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250723134806_BankAccounAndPaymentMetot', N'9.0.6');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250810112609_AddMaintenanceModeToSystemSettings', N'9.0.6');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250810113222_AddMaintenanceModeToSystemSettings2', N'9.0.6');

CREATE TABLE [SystemSettingses] (
    [Id] int NOT NULL IDENTITY,
    [IsConfigured] bit NOT NULL,
    [MaintenanceEnabled] bit NOT NULL,
    [MaintenanceMessage] nvarchar(max) NULL,
    [MaintenancePlannedEnd] datetime2 NULL,
    [MaintenanceAllowedRoles] nvarchar(max) NULL,
    [MaintenanceAllowedPaths] nvarchar(max) NULL,
    CONSTRAINT [PK_SystemSettingses] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250810113502_AddMaintenanceModeToSystemSettings3', N'9.0.6');

ALTER TABLE [Products] ADD [Sku] nvarchar(64) NOT NULL DEFAULT N'';

CREATE UNIQUE INDEX [IX_Products_Sku] ON [Products] ([Sku]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250812211049_ProductSKU', N'9.0.6');

ALTER TABLE [Products] ADD [Barcode] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250812221828_ProductBarkode', N'9.0.6');

CREATE TABLE [ContactMessages] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [Subject] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IpAddress] nvarchar(max) NULL,
    [UserAgent] nvarchar(max) NULL,
    [IsRead] bit NOT NULL,
    [Status] nvarchar(max) NULL,
    CONSTRAINT [PK_ContactMessages] PRIMARY KEY ([Id])
);

CREATE TABLE [ContactSettings] (
    [Id] int NOT NULL IDENTITY,
    [CompanyName] nvarchar(max) NULL,
    [AddressLine] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [Country] nvarchar(max) NULL,
    [Phone1] nvarchar(max) NULL,
    [Phone2] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [MapEmbedUrl] nvarchar(max) NULL,
    [WorkingHours] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_ContactSettings] PRIMARY KEY ([Id])
);

CREATE TABLE [SocialLinks] (
    [Id] int NOT NULL IDENTITY,
    [ContactSettingId] int NOT NULL,
    [Name] nvarchar(max) NULL,
    [Url] nvarchar(max) NULL,
    [IconCss] nvarchar(max) NULL,
    CONSTRAINT [PK_SocialLinks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SocialLinks_ContactSettings_ContactSettingId] FOREIGN KEY ([ContactSettingId]) REFERENCES [ContactSettings] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_SocialLinks_ContactSettingId] ON [SocialLinks] ([ContactSettingId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250817214757_ContactTable', N'9.0.6');

ALTER TABLE [CustomOrderRequests] ADD [BudgetMax] decimal(18,2) NULL;

ALTER TABLE [CustomOrderRequests] ADD [BudgetMin] decimal(18,2) NULL;

ALTER TABLE [CustomOrderRequests] ADD [DesiredDate] datetime2 NULL;

ALTER TABLE [CustomOrderRequests] ADD [EngravingText] nvarchar(max) NULL;

ALTER TABLE [CustomOrderRequests] ADD [Finish] nvarchar(max) NULL;

ALTER TABLE [CustomOrderRequests] ADD [Quantity] int NOT NULL DEFAULT 0;

ALTER TABLE [CustomOrderRequests] ADD [QuoteAmount] decimal(18,2) NULL;

ALTER TABLE [CustomOrderRequests] ADD [QuoteNote] nvarchar(max) NULL;

ALTER TABLE [CustomOrderRequests] ADD [QuotedAt] datetime2 NULL;

ALTER TABLE [CustomOrderRequests] ADD [Status] int NOT NULL DEFAULT 0;

ALTER TABLE [CustomOrderRequests] ADD [UpdatedAt] datetime2 NULL;

CREATE TABLE [CustomOrderAttachments] (
    [Id] int NOT NULL IDENTITY,
    [CustomOrderRequestId] int NOT NULL,
    [FileName] nvarchar(max) NULL,
    [FilePath] nvarchar(max) NULL,
    [ContentType] nvarchar(max) NULL,
    [FileSize] bigint NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_CustomOrderAttachments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomOrderAttachments_CustomOrderRequests_CustomOrderRequestId] FOREIGN KEY ([CustomOrderRequestId]) REFERENCES [CustomOrderRequests] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_CustomOrderAttachments_CustomOrderRequestId] ON [CustomOrderAttachments] ([CustomOrderRequestId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250818002458_CustomOrderAttachmentTable', N'9.0.6');

COMMIT;
GO


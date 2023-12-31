USE [master]
GO
/****** Object:  Database [DiplomBetaDB]    Script Date: 13.06.2023 19:38:58 ******/
CREATE DATABASE [DiplomBetaDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DiplomBetaDB', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DiplomBetaDB.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DiplomBetaDB_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\DiplomBetaDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [DiplomBetaDB] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DiplomBetaDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DiplomBetaDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DiplomBetaDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DiplomBetaDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DiplomBetaDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DiplomBetaDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DiplomBetaDB] SET  MULTI_USER 
GO
ALTER DATABASE [DiplomBetaDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DiplomBetaDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DiplomBetaDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DiplomBetaDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [DiplomBetaDB] SET DELAYED_DURABILITY = DISABLED 
GO
USE [DiplomBetaDB]
GO
/****** Object:  Table [dbo].[Carrier]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Carrier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Carrier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[TypeId] [int] NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Constraint]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Constraint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTask] [int] NOT NULL,
	[TypeConstraintId] [int] NOT NULL,
	[ProductCount] [int] NULL,
	[IdPoints] [nvarchar](50) NULL,
 CONSTRAINT [PK_Сonstraint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Point]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Point](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdClient] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ProductCount] [int] NOT NULL,
	[IdTask] [int] NOT NULL,
	[Position] [int] NOT NULL,
	[Address] [nvarchar](200) NULL,
 CONSTRAINT [PK_Point] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCarrier]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCarrier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdService] [int] NOT NULL,
	[IdCarrier] [int] NOT NULL,
	[Cost] [int] NOT NULL,
 CONSTRAINT [PK_ServiceCarrier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Task]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusId] [int] NOT NULL,
	[Cost] [int] NULL,
	[Conclusion] [nvarchar](max) NULL,
	[UserId] [int] NULL,
	[CarrierId] [int] NULL,
	[CountRow] [int] NULL,
	[CountColumn] [int] NULL,
 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaskService]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaskService](
	[IdTask] [int] NOT NULL,
	[IdService] [int] NOT NULL,
 CONSTRAINT [PK_TaskCost] PRIMARY KEY CLUSTERED 
(
	[IdTask] ASC,
	[IdService] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransportationCost]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransportationCost](
	[IdPosition] [int] NOT NULL,
	[IdTask] [int] NOT NULL,
	[Value] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TransportationCost_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeClient]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeClient](
	[id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TypeClient] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeConstraint]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeConstraint](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_TypeConstraint] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 13.06.2023 19:38:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Carrier] ON 

INSERT [dbo].[Carrier] ([Id], [Name], [Address], [Phone], [Email]) VALUES (8, N'Перевозки123', N'Улица Колотушкина 5', N'+7-(423)-423-2222', N'pewr@mail.ru')
INSERT [dbo].[Carrier] ([Id], [Name], [Address], [Phone], [Email]) VALUES (9, N'Dost', N'Улица Возильная 21', N'+7-(333)-212-7447', N'dost@mail.ru')
INSERT [dbo].[Carrier] ([Id], [Name], [Address], [Phone], [Email]) VALUES (10, N'Грузы быстро', N'Улица Дяктерева 57', N'+7-(288)-468-7894', N'sobaka@mail.ru')
SET IDENTITY_INSERT [dbo].[Carrier] OFF
SET IDENTITY_INSERT [dbo].[Client] ON 

INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (10, N'Арыш Мае', N'Улица Пушкина 70g', N'AR@mail.ru', 1)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (11, N'Пятерочкаа', N'Улица Вершина 2', N'Pyat@mail.ru', 1)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (12, N'Верный', N'Улица Красная 91', N'das@mail.ru', 1)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (13, N'Овощник', N'Улица Мира 21', N'Ovoshnik@mail.ru', 2)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (15, N'Мясник', N'Улица Комсомольская 23', N'Myasnik@mail.ru', 2)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (16, N'Орешник', N'Улица Липатова 9', N'Oreshnik@mail.ru', 2)
INSERT [dbo].[Client] ([Id], [CompanyName], [Address], [Email], [TypeId]) VALUES (22, N'Траффик', N'Улица Зубрева 7', N'Trafik@mail.ru', 1)
SET IDENTITY_INSERT [dbo].[Client] OFF
SET IDENTITY_INSERT [dbo].[Constraint] ON 

INSERT [dbo].[Constraint] ([Id], [IdTask], [TypeConstraintId], [ProductCount], [IdPoints]) VALUES (35, 29, 2, 10, N'2&2')
INSERT [dbo].[Constraint] ([Id], [IdTask], [TypeConstraintId], [ProductCount], [IdPoints]) VALUES (40, 29, 0, 10, N'1&1')
INSERT [dbo].[Constraint] ([Id], [IdTask], [TypeConstraintId], [ProductCount], [IdPoints]) VALUES (41, 29, 1, 10, N'1&2')
SET IDENTITY_INSERT [dbo].[Constraint] OFF
SET IDENTITY_INSERT [dbo].[Point] ON 

INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (94, 10, N'Магазин 1123c', 10, 4, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (95, 10, N'Магазинecqwe 2', 200, 4, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (96, 13, N'Склаqwvrqwд 1', 30, 4, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (97, 13, N'Склаbqrwrд 2', 500, 4, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (109, 10, N'Магазин 15', 90, 8, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (110, 10, N'Магазин 2', 203, 8, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (111, 13, N'Склад 1', 10, 8, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (112, 13, N'Склад 2', 524, 8, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (116, 13, N'Склад 1', 123, 10, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (117, 13, N'Склад 2', 123, 10, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (118, 22, N'Магазин 1', 1230, 10, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (119, 22, N'Магазин 2', 1230, 10, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (123, 16, N'Склад 1', 80, 11, 1, N'Московская 12а')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (124, 13, N'Склад 2', 666, 11, 2, N'Петропавловска 85')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (125, 13, N'Склад 3', 40, 11, 3, N'Новосибирска 82')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (189, 22, N'Магазин 3', 50, 10, 3, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (191, 10, N'Магазин 3', 50, 8, 3, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (245, 10, N'Магазин 1', 120, 11, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (246, 10, N'Магазин 2', 23, 11, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (249, 15, N'Склад 1', 123, 22, 1, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (250, 13, N'Склад 2w', 5, 22, 2, N'qwe')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (255, 10, N'Магазин 1', 5, 22, 1, N'Крутой')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (256, 10, N'Магазин 2', 1, 22, 2, N'')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (258, 15, N'Склад 3', 10, 22, 3, N'zxc')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (273, 10, N'Магазин 1', 20, 29, 1, N'qwe')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (274, 10, N'Магазин 2', 30, 29, 2, N'asd')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (275, 13, N'Склад 1', 40, 29, 1, N'zxc')
INSERT [dbo].[Point] ([Id], [IdClient], [Name], [ProductCount], [IdTask], [Position], [Address]) VALUES (276, 13, N'Склад 2', 50, 29, 2, N'bnm')
SET IDENTITY_INSERT [dbo].[Point] OFF
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([Id], [Name]) VALUES (1, N'Перевозка')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (2, N'Такелажные работы')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (3, N'Погрузка-Разгрузка')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (4, N'Страховка груза')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (5, N'Ответственное хранение')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (6, N'Срочная доставка')
INSERT [dbo].[Service] ([Id], [Name]) VALUES (7, N'Мягкая упаковка')
SET IDENTITY_INSERT [dbo].[Service] OFF
SET IDENTITY_INSERT [dbo].[ServiceCarrier] ON 

INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (13, 1, 8, 50)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (15, 3, 8, 150)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (16, 1, 9, 100)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (17, 2, 9, 200)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (18, 6, 9, 500)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (19, 1, 10, 200)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (20, 4, 10, 1000)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (21, 6, 10, 500)
INSERT [dbo].[ServiceCarrier] ([Id], [IdService], [IdCarrier], [Cost]) VALUES (23, 2, 8, 250)
SET IDENTITY_INSERT [dbo].[ServiceCarrier] OFF
INSERT [dbo].[Status] ([Id], [Name]) VALUES (1, N'Создана')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (2, N'Обработана')
INSERT [dbo].[Status] ([Id], [Name]) VALUES (3, N'В работе')
SET IDENTITY_INSERT [dbo].[Task] ON 

INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (4, 2, 1690, N'Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: 
       осуществить поставку груза из пункта «Склаqwvrqwд 1» в пункт «Магазин 1123c» в размере 10 единиц;
       осуществить поставку груза из пункта «Склаqwvrqwд 1» в пункт «Магазинecqwe 2» в размере 20 единиц;
       осуществить поставку груза из пункта «Склаbqrwrд 2» в пункт «Магазинecqwe 2» в размере 180 единиц;
       осуществить поставку груза из пункта «Склаbqrwrд 2» в пункт «Магазиrbqwrbн 3» в размере 20 единиц;

Стоимость провоза: 890
Сумма доставки перевозчика: 800

Итого: 1690

Неудовлетворенные потребности: 
       не удалось вывезти груз из пункта «Склаbqrwrд 2» в размере 300 единиц из-за недостатка запросов потребителей;

Общая нехватка: 300', 1, 10, 2, 2)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (8, 2, 3244, N'Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: 
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 2» в размере 10 единиц;
       осуществить поставку груза из пункта «Склад 2» в пункт «Магазин 15» в размере 90 единиц;
       осуществить поставку груза из пункта «Склад 2» в пункт «Магазин 2» в размере 193 единиц;

Стоимость провоза: 2144
Сумма доставки перевозчика: 300

Расходы на доп услуги: 
       Перевозка: 100
       Такелажные работы: 200
       Срочная доставка: 500

Итого: 3244', 1, 9, 2, 3)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (10, 2, NULL, NULL, 1, 10, 2, 3)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (11, 2, 900, N'Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: 
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 1» в размере 50 единиц;
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 2» в размере 10 единиц;
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 3» в размере 20 единиц;
       осуществить поставку груза из пункта «Склад 3» в пункт «Магазин 2» в размере 10 единиц;
       осуществить поставку груза из пункта «Склад 3» в пункт «Магазин 3» в размере 10 единиц;

Стоимость провоза: 350
Сумма доставки перевозчика: 250

Расходы на доп услуги: 
       Перевозка: 50
       Такелажные работы: 250

Итого: 900

Неудовлетворенные потребности: 
       не удалось вывезти груз из пункта «Склад 2» в размере 666 единиц из-за недостатка запросов потребителей;
       не удалось вывезти груз из пункта «Склад 3» в размере 20 единиц из-за недостатка запросов потребителей;

Общая нехватка: 686', 1, 8, 3, 2)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (22, 2, 245, N'Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: 
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 1» в размере 5 единиц;
       осуществить поставку груза из пункта «Склад 1» в пункт «Магазин 2» в размере 20 единиц;

Стоимость провоза: 45
Сумма доставки перевозчика: 200

Итого: 245

Неудовлетворенные потребности: 
       не удалось вывезти груз из пункта «Склад 1» в размере 98 единиц из-за недостатка запросов потребителей;
       не удалось вывезти груз из пункта «Склад 3» в размере 10 единиц из-за недостатка запросов потребителей;
       не удалось вывезти груз из пункта «Склад 2w» в размере 5 единиц из-за недостатка запросов потребителей;

Общая нехватка: 113', 1, 9, 3, 2)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (29, 2, 2200, N'Для минимальной стоимости перевозки груза необходимо осуществить следующие поставки: 
       осуществить поставку груза из пункта «Склад 1» по адресу zxc в пункт «Магазин 1» по адресу qwe в размере 20 единиц;
       осуществить поставку груза из пункта «Склад 1» по адресу zxc в пункт «Магазин 2» по адресу asd в размере 10 единиц;
       осуществить поставку груза из пункта «Склад 1» по адресу zxc в пункт «Магазин 2» по адресу asd в размере 10 единиц;
       осуществить поставку груза из пункта «Склад 2» по адресу bnm в пункт «Магазин 2» по адресу asd в размере 10 единиц;

Стоимость провоза: 200
Сумма доставки перевозчика: 800

Расходы на доп услуги: 
       Перевозка: 200
       Страховка груза: 1000

Итого: 2200

Неудовлетворенные потребности: 
       не удалось вывезти груз из пункта «Склад 2» в размере 40 единиц из-за недостатка запросов потребителей;

Общая нехватка: 40', 1, 10, 2, 2)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (30, 2, NULL, NULL, 2, 9, 0, 0)
INSERT [dbo].[Task] ([Id], [StatusId], [Cost], [Conclusion], [UserId], [CarrierId], [CountRow], [CountColumn]) VALUES (34, 1, NULL, NULL, 1, NULL, 0, 0)
SET IDENTITY_INSERT [dbo].[Task] OFF
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (4, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (8, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (8, 2)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (8, 6)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (10, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (10, 4)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (11, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (11, 2)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (22, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (29, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (29, 4)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (30, 1)
INSERT [dbo].[TaskService] ([IdTask], [IdService]) VALUES (34, 1)
SET IDENTITY_INSERT [dbo].[TransportationCost] ON 

INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 10, 1, 379)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 10, 2, 380)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 10, 3, 381)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 10, 4, 382)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (4, 10, 5, 383)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (5, 10, 6, 384)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 8, 1, 391)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 8, 2, 392)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 8, 3, 393)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 8, 0, 394)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 8, 4, 395)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (5, 8, 0, 396)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 4, 0, 499)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 4, 0, 500)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 4, 0, 501)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 4, 0, 502)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 11, 0, 532)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 11, 0, 533)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 11, 12, 534)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 11, 0, 535)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (4, 11, 0, 536)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (5, 11, 0, 537)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 22, 1, 538)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 22, 2, 539)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 22, 3, 540)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 22, 4, 541)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (4, 22, 5, 542)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (5, 22, 2, 543)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (0, 29, 1, 545)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (1, 29, 2, 546)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (2, 29, 3, 547)
INSERT [dbo].[TransportationCost] ([IdPosition], [IdTask], [Value], [Id]) VALUES (3, 29, 4, 548)
SET IDENTITY_INSERT [dbo].[TransportationCost] OFF
INSERT [dbo].[TypeClient] ([id], [Name]) VALUES (1, N'Заказчик')
INSERT [dbo].[TypeClient] ([id], [Name]) VALUES (2, N'Поставщик')
INSERT [dbo].[TypeConstraint] ([Id], [Name]) VALUES (0, N'Поставить не менее')
INSERT [dbo].[TypeConstraint] ([Id], [Name]) VALUES (1, N'Поставить не более')
INSERT [dbo].[TypeConstraint] ([Id], [Name]) VALUES (2, N'Поставить')
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Login], [Password], [Email]) VALUES (1, N'123', N'123', N'Nikita123@mail.ru')
INSERT [dbo].[User] ([Id], [Login], [Password], [Email]) VALUES (2, N'12345', N'12345', N'qwe@mail.ru')
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Task] ADD  CONSTRAINT [DF_Task_UserId]  DEFAULT ((0)) FOR [UserId]
GO
ALTER TABLE [dbo].[Client]  WITH CHECK ADD  CONSTRAINT [FK_Client_TypeClient] FOREIGN KEY([TypeId])
REFERENCES [dbo].[TypeClient] ([id])
GO
ALTER TABLE [dbo].[Client] CHECK CONSTRAINT [FK_Client_TypeClient]
GO
ALTER TABLE [dbo].[Constraint]  WITH CHECK ADD  CONSTRAINT [FK_Сonstraint_Task] FOREIGN KEY([IdTask])
REFERENCES [dbo].[Task] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Constraint] CHECK CONSTRAINT [FK_Сonstraint_Task]
GO
ALTER TABLE [dbo].[Constraint]  WITH CHECK ADD  CONSTRAINT [FK_Сonstraint_TypeConstraint] FOREIGN KEY([TypeConstraintId])
REFERENCES [dbo].[TypeConstraint] ([Id])
GO
ALTER TABLE [dbo].[Constraint] CHECK CONSTRAINT [FK_Сonstraint_TypeConstraint]
GO
ALTER TABLE [dbo].[Point]  WITH CHECK ADD  CONSTRAINT [FK_Point_Client] FOREIGN KEY([IdClient])
REFERENCES [dbo].[Client] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Point] CHECK CONSTRAINT [FK_Point_Client]
GO
ALTER TABLE [dbo].[Point]  WITH CHECK ADD  CONSTRAINT [FK_Point_Task] FOREIGN KEY([IdTask])
REFERENCES [dbo].[Task] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Point] CHECK CONSTRAINT [FK_Point_Task]
GO
ALTER TABLE [dbo].[ServiceCarrier]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCarrier_Carrier] FOREIGN KEY([IdCarrier])
REFERENCES [dbo].[Carrier] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceCarrier] CHECK CONSTRAINT [FK_ServiceCarrier_Carrier]
GO
ALTER TABLE [dbo].[ServiceCarrier]  WITH CHECK ADD  CONSTRAINT [FK_ServiceCarrier_Service] FOREIGN KEY([IdService])
REFERENCES [dbo].[Service] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ServiceCarrier] CHECK CONSTRAINT [FK_ServiceCarrier_Service]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Carrier] FOREIGN KEY([CarrierId])
REFERENCES [dbo].[Carrier] ([Id])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Carrier]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([Id])
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_Status]
GO
ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK_Task_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON UPDATE SET DEFAULT
GO
ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK_Task_User]
GO
ALTER TABLE [dbo].[TaskService]  WITH CHECK ADD  CONSTRAINT [FK_TaskService_Service] FOREIGN KEY([IdService])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[TaskService] CHECK CONSTRAINT [FK_TaskService_Service]
GO
ALTER TABLE [dbo].[TaskService]  WITH CHECK ADD  CONSTRAINT [FK_TaskService_Task] FOREIGN KEY([IdTask])
REFERENCES [dbo].[Task] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TaskService] CHECK CONSTRAINT [FK_TaskService_Task]
GO
ALTER TABLE [dbo].[TransportationCost]  WITH CHECK ADD  CONSTRAINT [FK_TransportationCost_Task] FOREIGN KEY([IdTask])
REFERENCES [dbo].[Task] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransportationCost] CHECK CONSTRAINT [FK_TransportationCost_Task]
GO
USE [master]
GO
ALTER DATABASE [DiplomBetaDB] SET  READ_WRITE 
GO

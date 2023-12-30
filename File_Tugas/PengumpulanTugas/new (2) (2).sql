USE [master]
GO
/****** Object:  Database [DB_PKKMB]    Script Date: 22/12/2023 14.15.26 ******/
CREATE DATABASE [DB_PKKMB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DB_PKKMB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DB_PKKMB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DB_PKKMB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DB_PKKMB_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DB_PKKMB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DB_PKKMB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DB_PKKMB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_PKKMB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_PKKMB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_PKKMB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_PKKMB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_PKKMB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DB_PKKMB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_PKKMB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_PKKMB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_PKKMB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DB_PKKMB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_PKKMB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_PKKMB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_PKKMB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_PKKMB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DB_PKKMB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_PKKMB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DB_PKKMB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DB_PKKMB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DB_PKKMB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_PKKMB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_PKKMB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DB_PKKMB] SET RECOVERY FULL 
GO
ALTER DATABASE [DB_PKKMB] SET  MULTI_USER 
GO
ALTER DATABASE [DB_PKKMB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DB_PKKMB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DB_PKKMB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DB_PKKMB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DB_PKKMB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DB_PKKMB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DB_PKKMB', N'ON'
GO
ALTER DATABASE [DB_PKKMB] SET QUERY_STORE = ON
GO
ALTER DATABASE [DB_PKKMB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [DB_PKKMB]
GO
/****** Object:  User [admin]    Script Date: 22/12/2023 14.15.26 ******/
CREATE USER [admin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  UserDefinedTableType [dbo].[StringList]    Script Date: 22/12/2023 14.15.26 ******/
CREATE TYPE [dbo].[StringList] AS TABLE(
	[Item] [varchar](10) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateAbsIdAbsensi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GenerateAbsIdAbsensi]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_absensi VARCHAR(10);

    SELECT @generated_id_absensi = 'ABS' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(abs_idabsensi, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_trabsensi;

    RETURN @generated_id_absensi;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateJdlIdJadwal]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GenerateJdlIdJadwal]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_jadwal VARCHAR(10);

    SELECT @generated_id_jadwal = 'JDL' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(jdl_idjadwal, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_msjadwal;

    RETURN @generated_id_jadwal;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateKelIdKelompok]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GenerateKelIdKelompok]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_kelompok VARCHAR(10);

    SELECT @generated_id_kelompok = 'KEL' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(kmk_idkelompok, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_mskelompok; -- Replace with your actual kelompok table name

    RETURN @generated_id_kelompok;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateKskNim]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GenerateKskNim]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_ksk_nim VARCHAR(10);

    SELECT @generated_ksk_nim = 'KSK' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(ksk_nim, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_mskesekretariatan;

    RETURN @generated_ksk_nim;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GeneratenlsNilaisikap]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREaTE FUNCTION [dbo].[GeneratenlsNilaisikap]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_nilaisikap VARCHAR(10);

    SELECT @generated_id_nilaisikap = 'NLS' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(nls_idnilaisikap, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_trnilaisikap;

    RETURN @generated_id_nilaisikap;
END;

GO
/****** Object:  UserDefinedFunction [dbo].[GenerateNopendaftaran]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GenerateNopendaftaran]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_nopendaftaran VARCHAR(10);

    SELECT @generated_nopendaftaran = 'MHS' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(mhs_nopendaftaran, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_msmahasiswa;

    RETURN @generated_nopendaftaran;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateNpkPicPkkmb]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GenerateNpkPicPkkmb]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_pkkmb VARCHAR(10);

    SELECT @generated_id_pkkmb = 'PIC' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(pic_nokaryawan, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_mspicpkkmb;

    RETURN @generated_id_pkkmb;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateRngIdRuangan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GenerateRngIdRuangan]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_ruangan VARCHAR(10);

    SELECT @generated_id_ruangan = 'RNG' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(rng_idruangan, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_msruangan; -- Gantilah nama_tabel_anda dengan nama tabel yang sesuai

    RETURN @generated_id_ruangan;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[GenerateTgsIdTugas]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GenerateTgsIdTugas]()
RETURNS VARCHAR(10)
AS
BEGIN
    DECLARE @generated_id_tugas VARCHAR(10);

    SELECT @generated_id_tugas = 'TGS' + RIGHT('000' + CAST(ISNULL(MAX(CAST(SUBSTRING(tgs_idtugas, 4, 3) AS INT)), 0) + 1 AS VARCHAR(3)), 3)
    FROM pkm_trtugas;

    RETURN @generated_id_tugas;
END;
GO
/****** Object:  Table [dbo].[pkm_mskesekretariatan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_mskesekretariatan](
	[ksk_nim] [varchar](10) NOT NULL,
	[ksk_nama] [varchar](100) NOT NULL,
	[ksk_jeniskelamin] [varchar](10) NOT NULL,
	[ksk_programstudi] [varchar](50) NOT NULL,
	[ksk_password] [text] NOT NULL,
	[ksk_role] [varchar](25) NOT NULL,
	[ksk_notelepon] [varchar](13) NOT NULL,
	[ksk_email] [varchar](50) NOT NULL,
	[ksk_alamat] [varchar](100) NOT NULL,
	[ksk_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_mskesekretariatan] PRIMARY KEY CLUSTERED 
(
	[ksk_nim] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  View [dbo].[view_KskDraft]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_KskDraft]
AS
SELECT  ksk_nim, ksk_nama, ksk_jeniskelamin, ksk_programstudi, ksk_password, ksk_role, ksk_notelepon, ksk_email, ksk_alamat, ksk_status
FROM      dbo.pkm_mskesekretariatan
WHERE   (ksk_status = 'Menunggu Verifikasi')
GO
/****** Object:  View [dbo].[view_KskAktif]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[view_KskAktif]
AS
SELECT  ksk_nim, ksk_nama, ksk_jeniskelamin, ksk_programstudi, ksk_password, ksk_role, ksk_notelepon, ksk_email, ksk_alamat, ksk_status
FROM      dbo.pkm_mskesekretariatan
WHERE   (ksk_status = 'Aktif')
GO
/****** Object:  Table [dbo].[pkm_dtlmahasiswainformasi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_dtlmahasiswainformasi](
	[dtl_informasi] [varchar](10) NOT NULL,
	[dtl_nopendaftaran] [varchar](10) NOT NULL,
 CONSTRAINT [PK_pkm_dtlmahasiswainformasi] PRIMARY KEY CLUSTERED 
(
	[dtl_informasi] ASC,
	[dtl_nopendaftaran] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_dtlmahasiswajudul]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_dtlmahasiswajudul](
	[dtl_idjadwal] [varchar](10) NOT NULL,
	[dtl_nopendaftaran] [varchar](10) NOT NULL,
 CONSTRAINT [PK_pkm_dtlmahasiswajudul] PRIMARY KEY CLUSTERED 
(
	[dtl_idjadwal] ASC,
	[dtl_nopendaftaran] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_msinformasi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_msinformasi](
	[inf_idinformasi] [varchar](10) NOT NULL,
	[inf_jenisinformasi] [varchar](50) NOT NULL,
	[inf_namainformasi] [varchar](100) NOT NULL,
	[inf_tglpublikasi] [datetime] NOT NULL,
	[inf_deskripsi] [varchar](100) NOT NULL,
	[inf_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_msinformasi] PRIMARY KEY CLUSTERED 
(
	[inf_idinformasi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_msjadwal]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_msjadwal](
	[jdl_idjadwal] [varchar](10) NOT NULL,
	[jdl_tglpelaksanaan] [datetime] NOT NULL,
	[jdl_waktupelaksanaan] [varchar](50) NOT NULL,
	[jdl_agenda] [varchar](100) NOT NULL,
	[jdl_tempat] [varchar](100) NOT NULL,
	[jdl_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_msjadwal] PRIMARY KEY CLUSTERED 
(
	[jdl_idjadwal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_mskelompok]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_mskelompok](
	[kmk_idkelompok] [varchar](10) NOT NULL,
	[kmk_namakelompok] [varchar](50) NOT NULL,
	[kmk_nim] [varchar](10) NOT NULL,
	[kmk_idruangan] [varchar](10) NOT NULL,
	[kmk_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_mskelompok] PRIMARY KEY CLUSTERED 
(
	[kmk_idkelompok] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_msmahasiswa]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_msmahasiswa](
	[mhs_nopendaftaran] [varchar](10) NOT NULL,
	[mhs_namalengkap] [varchar](100) NOT NULL,
	[mhs_gender] [varchar](10) NOT NULL,
	[mhs_programstudi] [varchar](50) NOT NULL,
	[mhs_alamat] [varchar](100) NOT NULL,
	[mhs_notelepon] [varchar](13) NOT NULL,
	[mhs_email] [varchar](50) NOT NULL,
	[mhs_password] [text] NOT NULL,
	[mhs_kategori] [varchar](30) NOT NULL,
	[mhs_idkelompok] [varchar](10) NULL,
	[mhs_idpkkmb] [varchar](10) NULL,
	[mhs_statuskelulusan] [varchar](20) NULL,
	[mhs_status] [varchar](20) NOT NULL,
	[mhs_saran] [text] NULL,
	[mhs_kritik] [text] NULL,
	[mhs_insight] [text] NULL,
	[mhs_tglkirimevaluasi] [date] NULL,
 CONSTRAINT [PK_pkm_msmahasiswa] PRIMARY KEY CLUSTERED 
(
	[mhs_nopendaftaran] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_mspicpkkmb]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_mspicpkkmb](
	[pic_nokaryawan] [varchar](10) NOT NULL,
	[pic_nama] [varchar](100) NOT NULL,
	[pic_password] [text] NOT NULL,
	[pic_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_mspicpkkmb] PRIMARY KEY CLUSTERED 
(
	[pic_nokaryawan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_mspkkmb]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_mspkkmb](
	[pkm_idPkkmb] [varchar](10) NOT NULL,
	[pkm_tahunPkkmb] [date] NOT NULL,
 CONSTRAINT [PK_pkm_mspkkmb] PRIMARY KEY CLUSTERED 
(
	[pkm_idPkkmb] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_msruangan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_msruangan](
	[rng_idruangan] [varchar](10) NOT NULL,
	[rng_namaruangan] [varchar](10) NOT NULL,
	[rng_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_msruangan] PRIMARY KEY CLUSTERED 
(
	[rng_idruangan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_trabsensi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_trabsensi](
	[abs_idabsensi] [varchar](10) NOT NULL,
	[abs_nim] [varchar](10) NULL,
	[abs_nopendaftaran] [varchar](10) NULL,
	[abs_tglkehadiran] [datetime] NULL,
	[abs_statuskehadiran] [varchar](20) NULL,
	[abs_keterangan] [varchar](100) NULL,
	[abs_status] [varchar](20) NULL,
 CONSTRAINT [PK_pkm_msabsensi] PRIMARY KEY CLUSTERED 
(
	[abs_idabsensi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_trdetailtugas]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_trdetailtugas](
	[dts_iddetail] [varchar](10) NOT NULL,
	[dts_nopendaftaram] [varchar](10) NOT NULL,
	[dts_filetugas] [text] NOT NULL,
	[dts_waktupengumpulam] [varchar](20) NOT NULL,
	[dts_nilaitugas] [float] NOT NULL,
 CONSTRAINT [PK_pkm_trdetailtugas] PRIMARY KEY CLUSTERED 
(
	[dts_iddetail] ASC,
	[dts_nopendaftaram] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_trnilaisikap]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_trnilaisikap](
	[nls_idnilaisikap] [varchar](10) NOT NULL,
	[nls_nopendaftaran] [varchar](10) NOT NULL,
	[nls_nim] [varchar](10) NOT NULL,
	[nls_sikap] [char](1) NULL,
	[nls_tanggal] [datetime] NOT NULL,
	[nls_jamplus] [int] NULL,
	[nls_jamminus] [int] NULL,
	[nls_deskripsi] [varchar](100) NULL,
	[nls_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_msnilaisikap] PRIMARY KEY CLUSTERED 
(
	[nls_idnilaisikap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[pkm_trtugas]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pkm_trtugas](
	[tgs_idtugas] [varchar](10) NOT NULL,
	[tgs_nim] [varchar](10) NOT NULL,
	[tgs_jenistugas] [varchar](30) NOT NULL,
	[tgs_tglpemberiantugas] [datetime] NOT NULL,
	[tgs_filetugas] [text] NOT NULL,
	[tgs_deadline] [datetime] NOT NULL,
	[tgs_deskripsi] [text] NULL,
	[tgs_status] [varchar](20) NOT NULL,
 CONSTRAINT [PK_pkm_mstugas] PRIMARY KEY CLUSTERED 
(
	[tgs_idtugas] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[pkm_dtlmahasiswainformasi]  WITH CHECK ADD  CONSTRAINT [FK_pkm_dtlmahasiswainformasi_pkm_msmahasiswa] FOREIGN KEY([dtl_nopendaftaran])
REFERENCES [dbo].[pkm_msmahasiswa] ([mhs_nopendaftaran])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_dtlmahasiswainformasi] CHECK CONSTRAINT [FK_pkm_dtlmahasiswainformasi_pkm_msmahasiswa]
GO
ALTER TABLE [dbo].[pkm_dtlmahasiswajudul]  WITH CHECK ADD  CONSTRAINT [FK_pkm_dtlmahasiswajudul_pkm_msmahasiswa] FOREIGN KEY([dtl_nopendaftaran])
REFERENCES [dbo].[pkm_msmahasiswa] ([mhs_nopendaftaran])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_dtlmahasiswajudul] CHECK CONSTRAINT [FK_pkm_dtlmahasiswajudul_pkm_msmahasiswa]
GO
ALTER TABLE [dbo].[pkm_mskelompok]  WITH CHECK ADD  CONSTRAINT [FK_pkm_mskelompok_pkm_mskesekretariatan] FOREIGN KEY([kmk_nim])
REFERENCES [dbo].[pkm_mskesekretariatan] ([ksk_nim])
GO
ALTER TABLE [dbo].[pkm_mskelompok] CHECK CONSTRAINT [FK_pkm_mskelompok_pkm_mskesekretariatan]
GO
ALTER TABLE [dbo].[pkm_mskelompok]  WITH CHECK ADD  CONSTRAINT [FK_pkm_mskelompok_pkm_msruangan] FOREIGN KEY([kmk_idruangan])
REFERENCES [dbo].[pkm_msruangan] ([rng_idruangan])
GO
ALTER TABLE [dbo].[pkm_mskelompok] CHECK CONSTRAINT [FK_pkm_mskelompok_pkm_msruangan]
GO
ALTER TABLE [dbo].[pkm_msmahasiswa]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msmahasiswa_pkm_mskelompok] FOREIGN KEY([mhs_idkelompok])
REFERENCES [dbo].[pkm_mskelompok] ([kmk_idkelompok])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_msmahasiswa] CHECK CONSTRAINT [FK_pkm_msmahasiswa_pkm_mskelompok]
GO
ALTER TABLE [dbo].[pkm_msmahasiswa]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msmahasiswa_pkm_mspkkmb] FOREIGN KEY([mhs_idpkkmb])
REFERENCES [dbo].[pkm_mspkkmb] ([pkm_idPkkmb])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_msmahasiswa] CHECK CONSTRAINT [FK_pkm_msmahasiswa_pkm_mspkkmb]
GO
ALTER TABLE [dbo].[pkm_trabsensi]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msabsensi_pkm_mskesekretariatan] FOREIGN KEY([abs_nim])
REFERENCES [dbo].[pkm_mskesekretariatan] ([ksk_nim])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_trabsensi] CHECK CONSTRAINT [FK_pkm_msabsensi_pkm_mskesekretariatan]
GO
ALTER TABLE [dbo].[pkm_trabsensi]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msabsensi_pkm_msmahasiswa] FOREIGN KEY([abs_nopendaftaran])
REFERENCES [dbo].[pkm_msmahasiswa] ([mhs_nopendaftaran])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_trabsensi] CHECK CONSTRAINT [FK_pkm_msabsensi_pkm_msmahasiswa]
GO
ALTER TABLE [dbo].[pkm_trdetailtugas]  WITH CHECK ADD  CONSTRAINT [FK_pkm_trdetailtugas_pkm_msmahasiswa] FOREIGN KEY([dts_nopendaftaram])
REFERENCES [dbo].[pkm_msmahasiswa] ([mhs_nopendaftaran])
GO
ALTER TABLE [dbo].[pkm_trdetailtugas] CHECK CONSTRAINT [FK_pkm_trdetailtugas_pkm_msmahasiswa]
GO
ALTER TABLE [dbo].[pkm_trdetailtugas]  WITH CHECK ADD  CONSTRAINT [FK_pkm_trdetailtugas_pkm_trtugas] FOREIGN KEY([dts_iddetail])
REFERENCES [dbo].[pkm_trtugas] ([tgs_idtugas])
GO
ALTER TABLE [dbo].[pkm_trdetailtugas] CHECK CONSTRAINT [FK_pkm_trdetailtugas_pkm_trtugas]
GO
ALTER TABLE [dbo].[pkm_trnilaisikap]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msnilaisikap_pkm_mskesekretariatan] FOREIGN KEY([nls_nim])
REFERENCES [dbo].[pkm_mskesekretariatan] ([ksk_nim])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_trnilaisikap] CHECK CONSTRAINT [FK_pkm_msnilaisikap_pkm_mskesekretariatan]
GO
ALTER TABLE [dbo].[pkm_trnilaisikap]  WITH CHECK ADD  CONSTRAINT [FK_pkm_msnilaisikap_pkm_msmahasiswa] FOREIGN KEY([nls_nopendaftaran])
REFERENCES [dbo].[pkm_msmahasiswa] ([mhs_nopendaftaran])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[pkm_trnilaisikap] CHECK CONSTRAINT [FK_pkm_msnilaisikap_pkm_msmahasiswa]
GO
ALTER TABLE [dbo].[pkm_trtugas]  WITH CHECK ADD  CONSTRAINT [FK_pkm_trtugas_pkm_mskesekretariatan] FOREIGN KEY([tgs_nim])
REFERENCES [dbo].[pkm_mskesekretariatan] ([ksk_nim])
GO
ALTER TABLE [dbo].[pkm_trtugas] CHECK CONSTRAINT [FK_pkm_trtugas_pkm_mskesekretariatan]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetInformasi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetInformasi]
    @inf_idinformasi VARCHAR(10) = NULL,
    @inf_jenisinformasi VARCHAR(50) = NULL,
    @inf_tglpublikasi_start DATETIME = NULL,
    @inf_tglpublikasi_end DATETIME = NULL,
    @inf_status VARCHAR(20) = NULL
AS
BEGIN
    SELECT
        inf_idinformasi,
        inf_jenisinformasi,
        inf_namainformasi,
        inf_tglpublikasi,
        inf_deskripsi,
        inf_status
    FROM
        pkm_msinformasi
    WHERE
        (@inf_idinformasi IS NULL OR inf_idinformasi = @inf_idinformasi)
        AND (@inf_jenisinformasi IS NULL OR inf_jenisinformasi = @inf_jenisinformasi)
        AND (@inf_tglpublikasi_start IS NULL OR inf_tglpublikasi >= @inf_tglpublikasi_start)
        AND (@inf_tglpublikasi_end IS NULL OR inf_tglpublikasi <= @inf_tglpublikasi_end)
        AND (@inf_status IS NULL OR inf_status = @inf_status);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertInformasi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertInformasi]
    @inf_idinformasi VARCHAR(10),
    @inf_jenisinformasi VARCHAR(50),
    @inf_namainformasi TEXT,
    @inf_tglpublikasi DATETIME,
    @inf_deskripsi TEXT,
	@inf_status VARCHAR(20)
AS
BEGIN
    INSERT INTO pkm_msinformasi (inf_idinformasi, inf_jenisinformasi, inf_namainformasi, inf_tglpublikasi, inf_deskripsi, inf_status)
    VALUES (@inf_idinformasi, @inf_jenisinformasi, @inf_namainformasi, @inf_tglpublikasi, @inf_deskripsi, @inf_status);
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertRuangan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertRuangan]
    @rng_namaruangan VARCHAR(10),
	@rng_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_msruangan (rng_idruangan, rng_namaruangan, rng_status)
        VALUES ([dbo].[GenerateRngIdRuangan](), @rng_namaruangan, @rng_status);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahAbsensi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahAbsensi]
    @p_nim VARCHAR(10),
    @p_nopendaftaran VARCHAR(10),
    @p_tglkehadiran DATETIME,
    @p_statuskehadiran VARCHAR(20),
    @p_keterangan VARCHAR(100),
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_trabsensi(
            abs_idabsensi,
            abs_nim,
            abs_nopendaftaran,
            abs_tglkehadiran,
            abs_statuskehadiran,
            abs_keterangan,
            abs_status
        )
        VALUES (
            dbo.GenerateAbsIdAbsensi(),
            @p_nim,
            @p_nopendaftaran,
            @p_tglkehadiran,
            @p_statuskehadiran,
            @p_keterangan,
            @p_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahJadwal]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahJadwal]
    @p_tglpelaksanaan DATETIME,
    @p_waktupelaksanaan VARCHAR(50),
    @p_agenda VARCHAR(100),
    @p_tempat VARCHAR(100),
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_msjadwal (
            jdl_idjadwal,
            jdl_tglpelaksanaan,
            jdl_waktupelaksanaan,
            jdl_agenda,
            jdl_tempat,
            jdl_status
        )
        VALUES (
            [dbo].[GenerateJdlIdJadwal](),
            @p_tglpelaksanaan,
            @p_waktupelaksanaan,
            @p_agenda,
            @p_tempat,
            @p_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahKelompok]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_TambahKelompok]
    @kmk_namakelompok VARCHAR(50),
    @kmk_nim VARCHAR(10),
    @kmk_idruangan VARCHAR(10),
    @kmk_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_mskelompok(
            kmk_idkelompok,
            kmk_namakelompok,
            kmk_nim,
            kmk_idruangan,
            kmk_status
        )
        VALUES (
            dbo.GenerateKelIdKelompok(),
            @kmk_namakelompok,
            @kmk_nim,
            @kmk_idruangan,
            @kmk_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahKesekretariatan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahKesekretariatan]
    @p_nama VARCHAR(100),
    @p_jeniskelamin VARCHAR(10),
    @p_programstudi VARCHAR(50),
    @p_password TEXT,
    @p_role VARCHAR(25),
	@p_notelepon VARCHAR(13),
    @p_email VARCHAR(50),
    @p_alamat VARCHAR(100),
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_mskesekretariatan (
            ksk_nim,
            ksk_nama,
            ksk_jeniskelamin,
            ksk_programstudi,
            ksk_password,
            ksk_role,
            ksk_notelepon,
            ksk_email,
            ksk_alamat,
            ksk_status
        )
        VALUES (
            dbo.GenerateKskNim(),
            @p_nama,
            @p_jeniskelamin,
            @p_programstudi,
            @p_password,
            @p_role,
            @p_notelepon,
            @p_email,
            @p_alamat,
            @p_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahMahasiswa]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahMahasiswa]
    @p_namalengkap VARCHAR(100),
    @p_gender VARCHAR(10),
    @p_programstudi VARCHAR(50),
    @p_alamat VARCHAR(100),
    @p_notelepon VARCHAR(13),
    @p_email VARCHAR(50),
    @p_password TEXT,
    @p_kategori VARCHAR(30),
	@p_statuskelulusan VARCHAR(20),
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_msmahasiswa (
            mhs_nopendaftaran,
            mhs_namalengkap,
            mhs_gender,
            mhs_programstudi,
            mhs_alamat,
            mhs_notelepon,
            mhs_email,
            mhs_password,
            mhs_kategori,
            mhs_statuskelulusan,
            mhs_status
        )
        VALUES (
            dbo.GenerateNopendaftaran(),
            @p_namalengkap,
            @p_gender,
            @p_programstudi,
            @p_alamat,
            @p_notelepon,
            @p_email,
            @p_password,
            @p_kategori,
            @p_statuskelulusan,
            @p_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahNilaisikap]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahNilaisikap]
    @p_nls_nopendaftaran VARCHAR(10),
	@p_nls_nim VARCHAR(10),
    @p_nls_sikap CHAR(1),
    @p_nls_tanggal DATETIME,
    @p_nls_jamplus INT,
    @p_nls_jamminus INT,
	@p_nls_deskripsi TEXT,
	@p_nls_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_trnilaisikap(
            nls_idnilaisikap,
            nls_nopendaftaran,
            nls_nim,
            nls_sikap,
            nls_tanggal,
            nls_jamplus,
            nls_jamminus,
			nls_deskripsi,
			nls_status
        )
        VALUES (
            dbo.GeneratenlsNilaisikap(),
            @p_nls_nopendaftaran,
            @p_nls_nim,
            @p_nls_sikap,
            @p_nls_tanggal,
            @p_nls_jamplus,
            @p_nls_jamminus,
			@p_nls_deskripsi,
			@p_nls_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahPicPkkmb]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahPicPkkmb]
    @pic_nama VARCHAR(100),
    @pic_password VARCHAR(100),
    @pic_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_mspicpkkmb(
			pic_nokaryawan,
            pic_nama,
            pic_password,
            pic_status
        )
        VALUES (
            dbo.GenerateNpkPicPkkmb(),
            @pic_nama,
            @pic_password,
            @pic_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_TambahTugas]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TambahTugas]
    @p_nim VARCHAR(10),
    @p_jenistugas VARCHAR(30),
    @p_tglpemberiantugas DATETIME,
    @p_filetugas TEXT,
    @p_deadline DATETIME,
    @p_deskripsi TEXT,
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO pkm_trtugas (
            tgs_idtugas,
            tgs_nim,
            tgs_jenistugas,
            tgs_tglpemberiantugas,
            tgs_filetugas,
            tgs_deadline,
            tgs_deskripsi,
            tgs_status
        )
        VALUES (
            [dbo].[GenerateTgsIdTugas](),
            @p_nim,
            @p_jenistugas,
            @p_tglpemberiantugas,
            @p_filetugas,
            @p_deadline,
            @p_deskripsi,
            @p_status
        );

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateInformasi]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateInformasi]
    @inf_idinformasi VARCHAR(10),
    @inf_jenisinformasi VARCHAR(50),
    @inf_namainformasi TEXT,
    @inf_tglpublikasi DATETIME,
    @inf_deskripsi TEXT,
	@inf_status VARCHAR(20)
AS
BEGIN
    UPDATE pkm_msinformasi
    SET
        inf_jenisinformasi = @inf_jenisinformasi,
        inf_namainformasi = @inf_namainformasi,
        inf_tglpublikasi = @inf_tglpublikasi,
        inf_deskripsi = @inf_deskripsi,
		inf_status = @inf_status
    WHERE
        inf_idinformasi = @inf_idinformasi;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateJadwal]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateJadwal]
    @p_idjadwal INT, -- Assuming jdl_idjadwal is an INT, adjust data type accordingly
    @p_tglpelaksanaan DATETIME,
    @p_waktupelaksanaan VARCHAR(50),
    @p_agenda VARCHAR(100),
    @p_tempat VARCHAR(100),
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE pkm_msjadwal
        SET
            jdl_tglpelaksanaan = @p_tglpelaksanaan,
            jdl_waktupelaksanaan = @p_waktupelaksanaan,
            jdl_agenda = @p_agenda,
            jdl_tempat = @p_tempat,
            jdl_status = @p_status
        WHERE
            jdl_idjadwal = @p_idjadwal;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateKelompok]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_UpdateKelompok]
    @kmk_idkelompok INT, -- Add parameter for Kelompok ID to update
    @kmk_namakelompok VARCHAR(50),
    @kmk_nim VARCHAR(10),
    @kmk_idruangan VARCHAR(10),
    @kmk_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Update the existing record in pkm_mskelompok
        UPDATE pkm_mskelompok
        SET
            kmk_namakelompok = @kmk_namakelompok,
            kmk_nim = @kmk_nim,
            kmk_idruangan = @kmk_idruangan,
            kmk_status = @kmk_status
        WHERE kmk_idkelompok = @kmk_idkelompok;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateNilaiSikap]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateNilaiSikap]
    @nls_idnilaisikap VARCHAR(10),
    @nls_nopendaftaran VARCHAR(10),
    @nls_nim VARCHAR(10),
    @nls_sikap CHAR(10),
    @nls_tanggal DATETIME,
	@nls_jamplus INT,
	@nls_jamminus INT,
	@nls_deskripsi VARCHAR(100),
	@nls_status VARCHAR(20)
AS
BEGIN
    UPDATE pkm_trnilaisikap
    SET
        nls_nopendaftaran = @nls_nopendaftaran,
        nls_nim =  @nls_nim,
        nls_sikap = @nls_sikap ,
		nls_tanggal = @nls_tanggal,
		nls_jamplus = @nls_jamplus,
		nls_jamminus = @nls_jamminus,
		nls_deskripsi = @nls_deskripsi,
		nls_status = @nls_status
    WHERE
        nls_idnilaisikap =  @nls_idnilaisikap;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateRuangan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateRuangan]
    @rng_idruangan VARCHAR(10),
    @rng_namaruangan VARCHAR(50)
AS
BEGIN
    UPDATE pkm_msruangan
    SET
        rng_namaruangan = @rng_namaruangan
    WHERE
        rng_idruangan = @rng_idruangan;
END;


-- Example of calling the INSERT stored procedure
EXEC sp_InsertRuangan 'Room001', 'Meeting Room A';

-- Example of calling the UPDATE stored procedure
EXEC sp_UpdateRuangan 'Room001', 'Updated Meeting Room A';
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateTugas]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_UpdateTugas]
    @p_idtugas VARCHAR(10), -- Assuming tgs_idtugas is an INT, adjust data type accordingly
    @p_nim VARCHAR(10),
    @p_jenistugas VARCHAR(30),
    @p_tglpemberiantugas DATETIME,
    @p_filetugas TEXT,
    @p_deadline DATETIME,
	@p_deskripsi TEXT,
    @p_status VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE pkm_trtugas
        SET
            tgs_nim = @p_nim,
            tgs_jenistugas = @p_jenistugas,
            tgs_tglpemberiantugas = @p_tglpemberiantugas,
            tgs_filetugas = @p_filetugas,
            tgs_deadline = @p_deadline,
            tgs_deskripsi = @p_deskripsi,
            tgs_status = @p_status
        WHERE
            tgs_idtugas = @p_idtugas;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_VerifikasiKesekretariatan]    Script Date: 22/12/2023 14.15.26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_VerifikasiKesekretariatan]
    @p_nim dbo.StringList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE pkm_mskesekretariatan SET ksk_status = 'Aktif' WHERE ksk_nim IN (SELECT Item FROM @p_nim);

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK;

        DECLARE @errorMessage NVARCHAR(4000);
        SET @errorMessage = ERROR_MESSAGE();

        THROW 50001, @errorMessage, 1;
    END CATCH;
END;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "pkm_mskesekretariatan"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_KskAktif'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_KskAktif'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "pkm_mskesekretariatan"
            Begin Extent = 
               Top = 7
               Left = 48
               Bottom = 170
               Right = 269
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_KskDraft'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'view_KskDraft'
GO
USE [master]
GO
ALTER DATABASE [DB_PKKMB] SET  READ_WRITE 
GO

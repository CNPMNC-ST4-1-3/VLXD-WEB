CREATE DATABASE QuanLy_VLXD_WEB;
GO
USE QuanLy_VLXD_WEB;
GO
-- Create NhanVien table
CREATE TABLE NhanVien (
    MaNV VARCHAR(10) PRIMARY KEY ,
    TenNV NVARCHAR(50),
    GioiTinh NVARCHAR(5),
    NgaySinh DATETIME,
    DiaChi NVARCHAR(50),
    SDT NVARCHAR(15),
    MatKhau NVARCHAR(30),
);
GO

-- Create KhachHang table
CREATE TABLE KhachHang (
    MaKH VARCHAR(10) PRIMARY KEY,
    HoTen NVARCHAR(100),
    NgaySinh DATE,
    GioiTinh NVARCHAR(10),
    DienThoai NVARCHAR(15),
    TaiKhoan NVARCHAR(50),
    MatKhau NVARCHAR(50),
    Email NVARCHAR(100),
    DiaChi NVARCHAR(MAX)
);
GO

-- Create NhaCungCap table
CREATE TABLE NhaCungCap (
    MaNCC VARCHAR(10) PRIMARY KEY,
    TenNCC NVARCHAR(50),
    Diachi NVARCHAR(50),
    SDT NVARCHAR(15)
);
GO

-- Create Loai table
CREATE TABLE Loai (
    MaLoai VARCHAR(10) PRIMARY KEY,
    TenLoai NVARCHAR(50),
    ThongTin NVARCHAR(100),
);
GO

-- Create HangHoa table
CREATE TABLE HangHoa (
    MaHH VARCHAR(10) PRIMARY KEY,
    TenHangHoa NVARCHAR(60),
    DonVi NVARCHAR(50),
	SoLuongTon INT,
    HinhAnh NVARCHAR(255),
    GiaBan INT,
    MaLoai VARCHAR(10) FOREIGN KEY REFERENCES Loai(MaLoai),
    MaNCC VARCHAR(10) FOREIGN KEY REFERENCES NhaCungCap(MaNCC)

);
GO

-- Tạo bảng DonBanHang
CREATE TABLE DonBanHang (
    MaDonBanHang VARCHAR(15) PRIMARY KEY,
    NgayGiao DATE,
    NgayDat DATE,
    NgayThanhToan DATE,
    MaKH VARCHAR(10),
	TongTien FLOAT,
    FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
GO	
-- Tạo bảng ChiTietDonHang
CREATE TABLE ChiTietDonBanHang(
    MaHH VARCHAR(10),
    MaDonBanHang VARCHAR(15),
    SoLuong INT,
    DonGia FLOAT,
	PRIMARY KEY (MaHH, MaDonBanHang),
    FOREIGN KEY (MaHH) REFERENCES HangHoa(MaHH),
    FOREIGN KEY (MaDonBanHang) REFERENCES DonBanHang(MaDonBanHang)
);
GO	

-- Create DonNhapHang table
CREATE TABLE DonNhapHang (
    MaDonNhapHang VARCHAR(15) PRIMARY KEY,
    NgayNhap DATE,
	MaNV VARCHAR(10),
	FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO
-- Create ChiTietDonNhapHang table
CREATE TABLE ChiTietDonNhapHang (
    MaHH VARCHAR(10),
	MaDonNhapHang VARCHAR(15),
    SoLuong INT,
    DonGia FLOAT,
	PRIMARY KEY (MaHH, MaDonNhapHang),
	FOREIGN KEY (MaHH) REFERENCES HangHoa(MaHH),
	FOREIGN KEY (MaDonNhapHang) REFERENCES DonNhapHang(MaDonNhapHang)
);
GO
-- Insert into NhanVien
INSERT INTO NhanVien (MaNV, TenNV, GioiTinh, NgaySinh, DiaChi, SDT, MatKhau) VALUES
('NV001', 'Nguyen Van A', 'Nam', '1990-01-01', '123 Le Loi', '0123456789',  'pass1'),
('NV002', 'Tran Thi B', 'Nu', '1992-02-02', '456 Hai Ba Trung', '0987654321',  'pass2'),
('NV003', 'Le Van C', 'Nam', '1985-03-03', '789 Nguyen Trai', '0912345678',  'pass3'),
('NV004', 'Pham Thi D', 'Nu', '1988-04-04', '1011 Tran Hung Dao', '0908765432',  'pass4'),
('NV005', 'Hoang Van E', 'Nam', '1980-05-05', '1213 Vo Thi Sau', '0934567890',  'pass5'),
('NV006', 'Nguyen Thi F', 'Nu', '1995-06-06', '1415 Ly Thuong Kiet', '0945678901',  'pass6'),
('NV007', 'Le Thi G', 'Nu', '1982-07-07', '1617 Ba Trieu', '0956789012', 'pass7');
GO
-- Insert into KhachHang
INSERT INTO KhachHang (MaKH, HoTen, NgaySinh, GioiTinh, DienThoai, TaiKhoan, MatKhau, Email, DiaChi) VALUES
('KH001', 'Nguyen Van H', '1980-01-01', 'Nam', '0901234567', 'nvh', 'passkh1', 'nvh@gmail.com', '123 Tan Binh'),
('KH002', 'Tran Thi I', '1985-02-02', 'Nu', '0912345678', 'tti', 'passkh2', 'tti@gmail.com', '456 Binh Thanh'),
('KH003', 'Le Van J', '1990-03-03', 'Nam', '0923456789', 'lvj', 'passkh3', 'lvj@gmail.com', '789 Thu Duc'),
('KH004', 'Pham Thi K', '1995-04-04', 'Nu', '0934567890', 'ptk', 'passkh4', 'ptk@gmail.com', '1011 Go Vap'),
('KH005', 'Hoang Van L', '2000-05-05', 'Nam', '0945678901', 'hvl', 'passkh5', 'hvl@gmail.com', '1213 Phu Nhuan'),
('KH006', 'Nguyen Thi M', '1985-06-06', 'Nu', '0956789012', 'ntm', 'passkh6', 'ntm@gmail.com', '1415 Binh Chanh'),
('KH007', 'Le Thi N', '1990-07-07', 'Nu', '0967890123', 'ltn', 'passkh7', 'ltn@gmail.com', '1617 District 1');
GO
-- Insert into NhaCungCap
INSERT INTO NhaCungCap (MaNCC, TenNCC, Diachi, SDT) VALUES
('NCC001', 'Cty Vat Lieu Xay Dung A', '123 Cong Hoa', '0901122334'),
('NCC002', 'Cty Vat Lieu Xay Dung B', '456 Le Van Sy', '0902233445'),
('NCC003', 'Cty Vat Lieu Xay Dung C', '789 Nguyen Kiem', '0903344556'),
('NCC004', 'Cty Vat Lieu Xay Dung D', '1011 Phan Xich Long', '0904455667'),
('NCC005', 'Cty Vat Lieu Xay Dung E', '1213 Phan Dinh Phung', '0905566778'),
('NCC006', 'Cty Vat Lieu Xay Dung F', '1415 Nguyen Van Troi', '0906677889'),
('NCC007', 'Cty Vat Lieu Xay Dung G', '1617 Nam Ky Khoi Nghia', '0907788990');
GO
-- Insert into DanhMucSanPham
INSERT INTO Loai(MaLoai, TenLoai, ThongTin) VALUES
('DM001', N'Gạch', N'Gạch xây dựng chất lượng cao' ),
('DM002', N'Xi măng', N'Xi măng Portland'),
('DM003', N'Cát', N'Cát xây dựng'),
('DM004', N'Đá', N'Đá xây dựng'),
('DM005', N'Thép', N'Thép cường lực'),
('DM006', N'Sơn', N'Sơn tường chất lượng'),
('DM007', N'Kính', N'Kính chịu lực');
GO
-- Insert into HangHoa
INSERT INTO HangHoa (MaHH, TenHangHoa, DonVi, SoLuongTon, HinhAnh, GiaBan, MaLoai, MaNCC) VALUES
('HH001', N'Xi Măng Xây Tô', N'Bao', 200, 'ximang_1.jpg', 85000, 'DM002', 'NCC002'),
('HH002', N'Xi Măng Fico', N'Bao', 300, 'ximang_2.jpg', 90000, 'DM002', 'NCC002'),
('HH003', N'Xi Măng Thăng Long', N'Bao', 400, 'ximang_3.jpg', 50000, 'DM002', 'NCC002'),
('HH004', N'Xi Măng Insee Đa Dụng', N'Bao', 200, 'ximang_4.jpg', 45000, 'DM002', 'NCC002'),
('HH005', N'Xi Măng Hà Tiên 1', N'Bao', 300, 'ximang_5.jpg', 80000, 'DM002', 'NCC002'),
('HH006', N'Xi Măng Hà TIên Miền Nam', N'Bao', 400, 'ximang_6.jpg', 750000, 'DM002', 'NCC002'),
('HH007', N'Xi Măng Xây Tô Hà Tiên 1', N'Bao', 100, 'ximang_7.jpg', 70000, 'DM002', 'NCC002'),
('HH008', N'Gạch Phước Thành', N'Viên', 200, 'gach_8.jpg', 50000, 'DM001', 'NCC001'),
('HH009', N'Gạch Thành Tâm', N'Viên', 300, 'gach_9.jpg', 70000, 'DM001', 'NCC001'),
('HH0010', N'Gạch Tám Quỳnh', N'Viên', 400, 'gach_10.jpg', 73000, 'DM001', 'NCC001'),
('HH0011', N'Cát bê tông hạt to ', N'M3', 310, 'cat_11.jpg', 320000, 'DM003', 'NCC003'),
('HH0012', N'Cát bê tông trộn', N'M3', 326, 'cat_12.jpg', 210000, 'DM003', 'NCC003'),
('HH0013', N'Cát san lấp', N'M3', 223, 'cat_13.jpg', 140000, 'DM003', 'NCC003'),
('HH0014', N'Cát xây tô', N'M3', 143, 'cat_14.jpg', 170000, 'DM003', 'NCC003'),
('HH0015', N'Đá 0 X 4 Xanh', N'M3', 200, 'da_15.jpg', 280000, 'DM004', 'NCC004'),
('HH0016', N'Đá 0 X 4 Xám Đen', N'M3', 200, 'da_16.jpg', 210000, 'DM004', 'NCC004'),
('HH0017', N'Đá 1 X 2 Xanh', N'M3', 200, 'da_17.jpg', 350000, 'DM004', 'NCC004'),
('HH0018', N'Đá 4 X 6 Xanh', N'M3', 200, 'da_18.jpg', 300000, 'DM004', 'NCC004'),
('HH0019', N'Thép Tấm', N'Cây', 100, 'thep_19.jpg', 200000, 'DM005', 'NCC005'),
('HH0020', N'Thép Hình', N'Cây', 100, 'thep_20.jpg', 200000, 'DM005', 'NCC005'),
('HH0021', N'Sơn Chống Thâm', N'Thùng', 250, 'son_21.jpg', 430000, 'DM006', 'NCC006'),
('HH0022', N'Sơn Tường MyColor', N'Thùng', 350, 'son_22.jpg', 230000, 'DM006', 'NCC006'),
('HH0023', N'Kính G', N'Tấm', 130, 'kinh_23.jpg', 111000, 'DM007', 'NCC007');
GO
-- Insert into DonBanHang
INSERT INTO DonBanHang (MaDonBanHang, NgayGiao, NgayDat, NgayThanhToan, TongTien, MaKH) VALUES
('DBH001', '2023-01-01', '2022-12-30', '2023-01-01', 5000,'KH001'),
('DBH002', '2023-01-02', '2022-12-31', '2023-01-02', 80000 ,'KH002'),
('DBH003', '2023-01-03', '2022-12-31', '2023-01-03',100000 ,'KH003'),
('DBH004', '2023-01-04', '2023-01-01', '2023-01-04',150000 ,'KH004'),
('DBH005', '2023-01-05', '2023-01-01', '2023-01-05',200000 ,'KH005'),
('DBH006', '2023-01-06', '2023-01-02', '2023-01-06',400000 , 'KH006'),
('DBH007', '2023-01-07', '2023-01-02', '2023-01-07',1000000 ,'KH007');
GO
-- Insert into ChiTietDonBanHang
INSERT INTO ChiTietDonBanHang (MaHH, MaDonBanHang, SoLuong, DonGia) VALUES
('HH001', 'DBH001', 100, 5000),
('HH002', 'DBH002', 50, 80000),
('HH003', 'DBH003', 20, 100000),
('HH004', 'DBH004', 10, 150000),
('HH005', 'DBH005', 5, 200000),
('HH006', 'DBH006', 2, 400000),
('HH007', 'DBH007', 1, 1000000);
GO
-- Insert into DonNhapHang
INSERT INTO DonNhapHang (MaDonNhapHang, NgayNhap, MaNV) VALUES
('DNH001', '2023-01-01', 'NV001'),
('DNH002', '2023-01-02', 'NV002'),
('DNH003', '2023-01-03', 'NV003'),
('DNH004', '2023-01-04', 'NV004'),
('DNH005', '2023-01-05', 'NV005'),
('DNH006', '2023-01-06', 'NV006'),
('DNH007', '2023-01-07', 'NV007');
GO
-- Insert into ChiTietDonNhapHang
INSERT INTO ChiTietDonNhapHang (MaHH, MaDonNhapHang, SoLuong, DonGia) VALUES
('HH001', 'DNH001', 100, 4500),
('HH002', 'DNH002', 50, 75000),
('HH003', 'DNH003', 20, 95000),
('HH004', 'DNH004', 10, 140000),
('HH005', 'DNH005', 5, 190000),
('HH006', 'DNH006', 2, 380000),
('HH007', 'DNH007', 1, 950000);
GO
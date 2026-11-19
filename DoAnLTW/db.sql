
INSERT INTO "Categories" ("CategoryName", "DanhmucMota")
SELECT v.name, v.mota
FROM (VALUES
    ('Rau củ', 'Các loại rau xanh, củ quả tươi sạch mỗi ngày'),
    ('Thịt', 'Thịt heo, thịt bò, thịt gà tươi ngon đạt chuẩn'),
    ('Bánh & Kẹo', 'Bánh quy, snack và các loại kẹo ngọt'),
    ('Sữa', 'Sữa tươi tiệt trùng, sữa chua và chế phẩm từ sữa'),
    ('Trái cây', 'Trái cây nội địa và nhập khẩu tươi ngon'),
    ('Đồ uống', 'Nước ép trái cây, trà và nước giải khát')
    
) AS v(name, mota)
WHERE NOT EXISTS (
    SELECT 1 FROM "Categories" c WHERE c."CategoryName" = v.name
);

INSERT INTO "Products" ("ProductName", "Price", "ImageUrl", "ProductMota", "SoLuongTon", "CategoryId")
SELECT p.name, p.price, p.img, p.mota, p.ton, c."CategoryId"
FROM (VALUES
    ('Cải thìa xanh 500g', 18000.0, 'https://images.unsplash.com/photo-1576045057995-568f588f82fb?w=600', 'Cải thìa tươi xanh canh tác theo tiêu chuẩn VietGAP, giòn ngọt tự nhiên.', 50, 'Rau củ'),
    ('Cà chua bi hữu cơ 500g', 25000.0, 'https://images.unsplash.com/photo-1546470427-e26264be0b0d?w=600', 'Cà chua bi chín mọng, giàu vitamin C, thích hợp ăn sống hoặc làm salad.', 40, 'Rau củ'),
    ('Xà lách Romaine 300g', 22000.0, 'https://images.unsplash.com/photo-1622206151226-18ca2c9ab4a1?w=600', 'Xà lách Romaine lá giòn, tươi mát, giữ nguyên độ tươi khi bảo quản lạnh.', 35, 'Rau củ'),
    ('Thịt ba rọi heo 500g', 75000.0, 'https://images.unsplash.com/photo-1607623814075-e51df1bdc82f?w=600', 'Thịt ba rọi heo tươi mới mỗi ngày, tỷ lệ nạc mỡ hài hòa cho món kho hoặc nướng.', 30, 'Thịt'),
    ('Ức gà phi lê 500g', 48000.0, 'https://images.unsplash.com/photo-1604503468506-a8da13d82791?w=600', 'Ức gà phi lê không xương, ít béo, giàu đạm, phù hợp thực đơn eat clean.', 45, 'Thịt'),
    ('Thịt bò thăn cắt lát 400g', 135000.0, 'https://images.unsplash.com/photo-1588168333986-5078d3ae3976?w=600', 'Thịt bò thăn mềm ngọt tự nhiên, thái lát vừa ăn cho món xào hoặc áp chảo.', 20, 'Thịt'),
    ('Bánh quy bơ Danisa 454g', 115000.0, 'https://images.unsplash.com/photo-1558961363-fa8fdf82db35?w=600', 'Bánh quy bơ công thức Đan Mạch thơm béo, hộp thiếc sang trọng.', 25, 'Bánh & Kẹo'),
    ('Snack khoai tây vị tự nhiên 160g', 32000.0, 'https://images.unsplash.com/photo-1566478989037-eec170784d0b?w=600', 'Snack khoai tây lát mỏng giòn rụm, đậm đà hương vị khoai tây tự nhiên.', 60, 'Bánh & Kẹo'),
    ('Sữa tươi tiệt trùng ít đường 1L', 34000.0, 'https://images.unsplash.com/photo-1563636619-e9143da7973b?w=600', 'Sữa tươi tiệt trùng 100% từ sữa bò tươi nguyên chất, bổ sung canxi và vitamin D.', 80, 'Sữa'),
    ('Sữa chua uống lên men 65ml x 5', 28000.0, 'https://images.unsplash.com/photo-1488477181946-6428a0291777?w=600', 'Sữa chua uống lên men tự nhiên hỗ trợ tiêu hóa và tăng cường đề kháng.', 70, 'Sữa'),
    ('Táo Envy nhập khẩu 500g', 69000.0, 'https://images.unsplash.com/photo-1560806887-1e4cd0b6cbd6?w=600', 'Táo Envy vỏ đỏ sọc vàng bắt mắt, thịt giòn tan, vị ngọt đậm thơm dịu.', 40, 'Trái cây'),
    ('Nước cam ép nguyên chất 1L', 45000.0, 'https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=600', 'Nước cam ép từ cam tươi chọn lọc, không chất bảo quản, giải nhiệt sảng khoái.', 55, 'Đồ uống')
    
) AS p(name, price, img, mota, ton, cat_name)
JOIN "Categories" c ON c."CategoryName" = p.cat_name
WHERE NOT EXISTS (
    SELECT 1 FROM "Products" pr WHERE pr."ProductName" = p.name
);


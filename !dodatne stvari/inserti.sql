
INSERT INTO `mydb`.`zaposleni` (`IdZaposlenog`, `Ime`, `Prezime`, `Email`, `Lozinka`, `Uloga`, `Datum_zaposlenja`, `Aktivan`) VALUES (1, 'Marko', 'Markovic', 'markovic@gmail.com', 'f18d6b99c2220efb480af983c37ddd65ba0262fe72b55e3ea67d4288798c1374)', 'Menadzer', '2023-01-05', DEFAULT);
INSERT INTO `mydb`.`zaposleni` (`IdZaposlenog`, `Ime`, `Prezime`, `Email`, `Lozinka`, `Uloga`, `Datum_zaposlenja`, `Aktivan`) VALUES (2, 'Petar', 'Petrovic', 'petrovic@gmail.com', '20d3ebb9526c1fd137189ffe7eebabfb0505ac1e553875a46db6ca23fdf68017)', 'Prodavac', '2023-02-03', DEFAULT);
INSERT INTO `mydb`.`zaposleni` (`IdZaposlenog`, `Ime`, `Prezime`, `Email`, `Lozinka`, `Uloga`, `Datum_zaposlenja`, `Aktivan`) VALUES (3, 'Milos', 'Milosevic', 'milosevic@gmail.com', '76d11b122320bc5f9bc7e5f889a76e13bbed8e2aab0418d4a6b82f319b6e2bd3)', 'Prodavac', '2023-03-04', DEFAULT);

INSERT INTO `mydb`.`kupac` (`IdKupca`, `Ime`, `Prezime`, `Telefon`) VALUES ('1', 'Jovan', 'Jovanovic', '065222111');
INSERT INTO `mydb`.`kupac` (`IdKupca`, `Ime`, `Prezime`, `Telefon`) VALUES ('2', 'Mirka', 'Mirkovic', '066444555');
INSERT INTO `mydb`.`kupac` (`IdKupca`, `Ime`, `Prezime`, `Telefon`) VALUES ('3', 'Uros', 'Urosevic', '066999888');

INSERT INTO `mydb`.`kategorija_proizvoda` (`IdKategorije`, `Naziv`, `Opis`) VALUES ('1', 'Laptopi', 'prenosni racunari');
INSERT INTO `mydb`.`kategorija_proizvoda` (`IdKategorije`, `Naziv`, `Opis`) VALUES ('2', 'Misevi', 'opticki i bezicni');
INSERT INTO `mydb`.`kategorija_proizvoda` (`IdKategorije`, `Naziv`) VALUES ('3', 'Monitori');

INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('1', 'Lenovo ThinkPad', '10', '1500.00', '14 inch, 16GB RAM, 512GB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('2', 'Dell XPS 13', '5', '1200.00', '13 inch, 16GB RAM, 256GB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('3', 'Logitech g502', '15', '100.00', 'bezicni gejmerski mis', '2');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('4', 'Samsung 24\" LED', '8', '200.00', 'Full HD monitor, 60Hz', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('5', 'HP Omen 27\"', '3', '450.00', 'Gaming monitor, QHD, 165HZ', '3');

# inserti dodati
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('6', 'Apple MacBook Air', '12', '1300.00', '13 inch, 8GB RAM, 256GB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('7', 'Asus ROG Zephyrus', '7', '2000.00', '15 inch, 32GB RAM, 1TB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('8', 'Razer DeathAdder', '20', '80.00', 'gejmerski mis, 6400 DPI', '2');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('9', 'Acer Predator XB3', '4', '500.00', '27 inch, 240Hz, G-Sync', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('10', 'Logitech K380', '25', '40.00', 'bežična tastatura', '2');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('11', 'LG UltraGear 27GL850', '6', '400.00', '27 inch, 144Hz, Nano IPS', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('12', 'Microsoft Surface Pro 7', '8', '900.00', '12.3 inch, 8GB RAM, 128GB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('13', 'Corsair K95 RGB', '10', '150.00', 'mehanička tastatura', '2');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('14', 'BenQ PD3220U', '3', '1000.00', '32 inch, 4K HDR monitor', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('15', 'Acer Nitro 5', '5', '950.00', '15.6 inch, 16GB RAM, 512GB SSD', '1');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('16', 'Dell UltraSharp U2720Q', '7', '700.00', '27 inch, 4K monitor', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('17', 'Apple Magic Mouse', '14', '80.00', 'bežični miš', '2');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('18', 'Samsung Odyssey G9', '2', '1500.00', '49 inch, 240Hz, QLED', '3');
INSERT INTO `mydb`.`proizvod` (`IdProizvoda`, `Naziv`, `Kolicina`, `Cijena`, `Opis`, `IdKategorije`) VALUES ('19', 'Logitech MX Master 3', '22', '100.00', 'bežični miš za produktivnost', '2');

INSERT INTO `mydb`.`faktura` (`BrojFakture`, `Datum_Izdavanja`, `Ukupan_Iznos`, `IdKupca`, `IdProdavca`) VALUES ('101', '2024-10-10', '1600.00', '1', '2');
INSERT INTO `mydb`.`faktura` (`BrojFakture`, `Datum_Izdavanja`, `Ukupan_Iznos`, `IdKupca`, `IdProdavca`) VALUES ('102', '2024-10-11', '1400.00', '2', '3');
INSERT INTO `mydb`.`faktura` (`BrojFakture`, `Datum_Izdavanja`, `Ukupan_Iznos`, `IdKupca`, `IdProdavca`) VALUES ('103', '2024-10-11', '200.00', '3', '2');

INSERT INTO `mydb`.`stavka_fakture` (`BrojFakture`, `IdProizvoda`, `Kolicina`, `Iznos`) VALUES ('101', '1', '1', '1500.00');
INSERT INTO `mydb`.`stavka_fakture` (`BrojFakture`, `IdProizvoda`, `Kolicina`, `Iznos`) VALUES ('101', '3', '1', '100.00');
INSERT INTO `mydb`.`stavka_fakture` (`BrojFakture`, `IdProizvoda`, `Kolicina`, `Iznos`) VALUES ('102', '2', '1', '1200.00');
INSERT INTO `mydb`.`stavka_fakture` (`BrojFakture`, `IdProizvoda`, `Kolicina`, `Iznos`) VALUES ('102', '4', '1', '200.00');
INSERT INTO `mydb`.`stavka_fakture` (`BrojFakture`, `IdProizvoda`, `Kolicina`, `Iznos`) VALUES ('103', '4', '1', '200.00');

INSERT INTO `mydb`.`dobavljac` (`IdDobavljaca`, `Naziv`, `Email`) VALUES ('1', 'Lenovo RS', 'lenovo@lenovo.org');
INSERT INTO `mydb`.`dobavljac` (`IdDobavljaca`, `Naziv`, `Email`) VALUES ('2', 'IT Distribucija', 'distribucija@gmail.com');

INSERT INTO `mydb`.`nabavka` (`IdNabavke`, `Datum_Nabavke`, `Ukupan_Iznos`, `IdMenadzera`, `IdDobavljaca`) VALUES ('1', '2024-11-01', '7500.00', '1', '1');
INSERT INTO `mydb`.`nabavka` (`IdNabavke`, `Datum_Nabavke`, `Ukupan_Iznos`, `IdMenadzera`, `IdDobavljaca`) VALUES ('2', '2024-11-01', '3500.00', '1', '2');

INSERT INTO `mydb`.`stavka_nabavke` (`Kolicina`, `Iznos`, `IdProizvoda`, `IdNabavke`) VALUES ('5', '7500.00', '1', '1');
INSERT INTO `mydb`.`stavka_nabavke` (`Kolicina`, `Iznos`, `IdProizvoda`, `IdNabavke`) VALUES ('2', '2400.00', '2', '2');
INSERT INTO `mydb`.`stavka_nabavke` (`Kolicina`, `Iznos`, `IdProizvoda`, `IdNabavke`) VALUES ('3', '300.00', '3', '2');
INSERT INTO `mydb`.`stavka_nabavke` (`Kolicina`, `Iznos`, `IdProizvoda`, `IdNabavke`) VALUES ('4', '800.00', '4', '2');

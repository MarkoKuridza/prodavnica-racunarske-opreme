-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8mb4 DEFAULT COLLATE utf8mb4_0900_ai_ci;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`Zaposleni`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Zaposleni` (
  `IdZaposlenog` INT NOT NULL AUTO_INCREMENT, 
  `Ime` VARCHAR(45) NOT NULL,
  `Prezime` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Lozinka` VARCHAR(255) NOT NULL,
  `Uloga` ENUM('Prodavac', 'Menadzer') NOT NULL,
  `Datum_zaposlenja` DATE NOT NULL,
  `Aktivan` TINYINT NOT NULL DEFAULT 1,
  PRIMARY KEY (`IdZaposlenog`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Kupac`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Kupac` (
  `IdKupca` INT NOT NULL AUTO_INCREMENT,
  `Ime` VARCHAR(45) NOT NULL,
  `Prezime` VARCHAR(45) NOT NULL,
  `Telefon` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`IdKupca`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Faktura`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Faktura` (
  `BrojFakture` INT NOT NULL AUTO_INCREMENT,
  `Datum_Izdavanja` DATE NOT NULL,
  `Ukupan_Iznos` DECIMAL(7,2) NOT NULL,
  `IdKupca` INT NOT NULL,
  `IdProdavca` INT NOT NULL,
  PRIMARY KEY (`BrojFakture`),
  INDEX `fk_Narudzba_Kupac_idx` (`IdKupca` ASC) VISIBLE,
  INDEX `fk_Faktura_Zaposleni1_idx` (`IdProdavca` ASC) VISIBLE,
  CONSTRAINT `fk_Narudzba_Kupac`
    FOREIGN KEY (`IdKupca`)
    REFERENCES `mydb`.`Kupac` (`IdKupca`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Faktura_Zaposleni1`
    FOREIGN KEY (`IdProdavca`)
    REFERENCES `mydb`.`Zaposleni` (`IdZaposlenog`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Kategorija_Proizvoda`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Kategorija_Proizvoda` (
  `IdKategorije` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Opis` VARCHAR(200) NULL,
  PRIMARY KEY (`IdKategorije`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Proizvod`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Proizvod` (
  `IdProizvoda` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Kolicina` INT NOT NULL,
  `Cijena` DECIMAL(6,2) NOT NULL,
  `Opis` VARCHAR(200) NOT NULL,
  `IdKategorije` INT NOT NULL,
  PRIMARY KEY (`IdProizvoda`),
  INDEX `fk_Proizvod_Kategorija_Proizvoda1_idx` (`IdKategorije` ASC) VISIBLE,
  CONSTRAINT `fk_Proizvod_Kategorija_Proizvoda1`
    FOREIGN KEY (`IdKategorije`)
    REFERENCES `mydb`.`Kategorija_Proizvoda` (`IdKategorije`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Stavka_Fakture`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Stavka_Fakture` (
  `BrojFakture` INT NOT NULL,
  `IdProizvoda` INT NOT NULL,
  `Kolicina` INT NOT NULL,
  `Iznos` DECIMAL(7,2) NOT NULL,
  PRIMARY KEY (`BrojFakture`, `IdProizvoda`),
  INDEX `fk_Stavka_Narudzbe_Proizvod1_idx` (`IdProizvoda` ASC) VISIBLE,
  CONSTRAINT `fk_Stavka_Narudzbe_Narudzba1`
    FOREIGN KEY (`BrojFakture`)
    REFERENCES `mydb`.`Faktura` (`BrojFakture`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Stavka_Narudzbe_Proizvod1`
    FOREIGN KEY (`IdProizvoda`)
    REFERENCES `mydb`.`Proizvod` (`IdProizvoda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Dobavljac`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Dobavljac` (
  `IdDobavljaca` INT NOT NULL AUTO_INCREMENT,
  `Naziv` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`IdDobavljaca`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Nabavka`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Nabavka` (
  `IdNabavke` INT NOT NULL AUTO_INCREMENT,
  `Datum_Nabavke` DATE NOT NULL,
  `Ukupan_Iznos` DECIMAL(7,2) NOT NULL,
  `IdMenadzera` INT NOT NULL,
  `IdDobavljaca` INT NOT NULL,
  PRIMARY KEY (`IdNabavke`),
  INDEX `fk_Nabavka_Zaposleni1_idx` (`IdMenadzera` ASC) VISIBLE,
  INDEX `fk_Nabavka_Dobavljac1_idx` (`IdDobavljaca` ASC) VISIBLE,
  CONSTRAINT `fk_Nabavka_Zaposleni1`
    FOREIGN KEY (`IdMenadzera`)
    REFERENCES `mydb`.`Zaposleni` (`IdZaposlenog`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Nabavka_Dobavljac1`
    FOREIGN KEY (`IdDobavljaca`)
    REFERENCES `mydb`.`Dobavljac` (`IdDobavljaca`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`Stavka_Nabavke`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`Stavka_Nabavke` (
  `Kolicina` INT NOT NULL,
  `Iznos` DECIMAL(7,2) NOT NULL,
  `IdProizvoda` INT NOT NULL,
  `IdNabavke` INT NOT NULL,
  PRIMARY KEY (`IdProizvoda`, `IdNabavke`),
  INDEX `fk_Stavka_Nabavke_Proizvod1_idx` (`IdProizvoda` ASC) VISIBLE,
  INDEX `fk_Stavka_Nabavke_Nabavka1_idx` (`IdNabavke` ASC) VISIBLE,
  CONSTRAINT `fk_Stavka_Nabavke_Proizvod1`
    FOREIGN KEY (`IdProizvoda`)
    REFERENCES `mydb`.`Proizvod` (`IdProizvoda`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Stavka_Nabavke_Nabavka1`
    FOREIGN KEY (`IdNabavke`)
    REFERENCES `mydb`.`Nabavka` (`IdNabavke`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
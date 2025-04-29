# KRATAK OPIS

  WPF projekat "Prodavnica racunarske opreme" rađen u Visual Studiu 2017 sa .net verzijom 4.7.2.
  Projekat je rađen za potrebe predmeta "Iterakcija čovjek-računar" sa ciljem da se izuči WPF tehNologija.

  Aplikacija ima podršku za dva tipa korisničkih naloga:
  1. Prodavac
     Funkcionalnosti koje prodavac ima su:
     - pregled svih proizvoda,
     - filtriranje proizvoda po id-u, nazivu i kategoriji,
     - kreiranje faktura i
     - pregled svih faktura i njihovih detalja
  2. Menadžer
     Funkcionalnostio koje menadžer ima su:
     - upravljanje zaposlenih (izmjena podataka, aktivnosti),
     - dodavanje novih zaposlenih,
     - dodavanje novih proizvoda i kategorija proizvoda,
     - kreiranje nabavke
     - pregled svih nabavki i njihovih detalja
  Zajedničko za oba naloga jesu podešavanja koja uključuju promjenu jezika.

# KORISNIČKO UPUTSTVO
## LOG IN PROZOR
  Prilikom pokretanja aplikacija otvara se Log in prozor gdje se korisnik sistema prijavljuje. 
  Korisnik ima na raspolaganju opciju da promijeni zadati jezik u srpski ili engleski klikom na Menu 'Language'/'Jezik'.
  
  U polja unosi svoje ime i lozinku. Za uneseno ispravno ime i lozinku, u zavisnosti od njegove uloge koja mu je dodjeljena, korisniku se otvara prozor za prodavca, ako mu je dodjeljena uloga 'Prodavac' ili prozor za menadžera, ako mu je dodjeljena uloga 'Menadzer'.
  
  Za uneseno neispravno ime ili lozinku sistem će odbiti prijavu i prikazati poruku neuspješne prijave. Takođe, korisnik može da bude neaktivan, odnosno da mu nije dozvoljen pristup sistemu. Ako takav korisnik pokuša da se prijavi na sistem prikazaće mu se poruka da je neaktivan.
  
  
  ![log in widnow](https://github.com/user-attachments/assets/9302c922-2293-45b9-bde1-fd03ae7fdaa0)

## SELLER/PRODAVAC PROZOR
Kada se korisnik koji ima ulogu prodavca uspješno prijavi na sistem prikazaće mu se prozor sa početnim Tab-om 'Products'/'Proizvodi'.
Ostali Tab-ovi koje korisnik ima na raspolaganju su: 
- Create invoice / Kreiraj fakturu,
- View All Invoices / Sve fakture i
- Settings / Podešavanja.


U Tab-u 'Products' su prikazani svi proizvodi koji trenutno postoje sa njihovim detaljima.

![seller window  - products tab](https://github.com/user-attachments/assets/06d41bec-b75d-4443-8254-13ef5782472f)

Prodavcu su na raspolaganju opcije za filtriranje proizvoda po ID-u, Nazivu i Kategoriji.
Prozor služi samo za pregled proizvoda.

U Tab-u 'Create invoice' prodavac kreira fakturu.

![seller window  - create invoice tab](https://github.com/user-attachments/assets/1d45c8c1-5c8d-4478-9a7b-3881077f0775)

U dijelu 'Avaible Products' se nalaze trenutno dostupni proizvodi koji mogu da se dodaju u fakturu. Proizvodi mogu da se pretražuju po njihovom nazivu.

Kada prodavac želi da doda proizvod u fakturu, pored detalja proizvoda postoji dugme '+' koje podrazumijevano dodaje 1 proizvod u fakturu. Klikom više puta na dugme '+' za isti proizvod, povećava se količina proizvoda u fakturi. Ako prodavac želi da doda više proizvoda odjednom, pored dugmeta '+' može da promijeni podrazumijevanu količinu u željenu. Sistem onemogućava da se dodaje količina proizvoda veća od dostupne količine proizvoda.

U dijelu 'Invoice Items' se nalaze proizvodi dodati u fakturu.

![seller window  - create invoice tab 2](https://github.com/user-attachments/assets/64fbee0d-a9a0-4448-92ee-5d5f18e14987)

Prodavac može da ukloni proizvod iz fakture klikom da dugme '-' koje se nalazi pored proizovda u fakturi.

U dijelu 'Invoice Details' se unose podaci kupca: ime, prezime i broj telefona. Takođe, nalaze se informacije o datumu izdavanja i ukupnom iznosu fakture.

![seller window  - create invoice tab 3](https://github.com/user-attachments/assets/e8d16ac6-4250-44de-8fc7-8ac75266d3cc)

Kada prodavac želi da sačuva fakturu, to vrši klikom na dugme 'Save invoice' i prikaže se poruka o uspješno sačuvanoj fakturi.
Obavezno je da se sva polja vezana za kupca popune kao i da postoje proizvodi u fakturi kako bi se faktura sačuvala, u suprotnom prikazaće se greška da se nisu sva polja popunila.

U Tab-u 'View All Invoices' su prikazane sve fakture.

![seller window  - view all invoices tab](https://github.com/user-attachments/assets/80367ad6-3818-4b3e-aaaa-8531c44590a7)

U dijelu 'Invoices' se nalaze sve fakture sa njivovim brojem i datumom izdavanja.

Selekcijom jedne od faktura, u dijelu 'Invoice Details' prikazaće se detalji selektovane fakture.

![seller window  - view all invoices tab 2](https://github.com/user-attachments/assets/dab0904d-627d-4503-b169-63a998e5cca9)

Detalji koji su prikazani su: detalji prodavca koji je izdao fakturu, detalji kupca i svi proizvodi koji se nalaze u fakturi.

U dijelu 'Settings' se nalaze podešavanja koja su na raspolaganju prodavcu, a to je promjena jezika u englesi ili srpski. Promjena teme nije implementirana!

## MANAGER PROZOR

Kada se korisnik koji ima ulogu menadžera uspješno prijavi na sistem prikazaće mu se prozor sa početnim Tab-om 'Edit Employee'/'Uredi Zaposlenog'.
Ostali Tab-ovu koji su na raspolaganju su:
  - Add Employee / Dodaj Zaposlenog,
  - Add Products / Dodaj Proizvod,
  - Order From A Supplier / Naruči Od Dobavljača,
  - View All Orders / Sve Nabavke i
  - Settings / Podešavanja.

U Tab-u 'Edit Employee' su prikazani svi zaposleni sa njihovim detaljima.

![manager window  - edit employee tab](https://github.com/user-attachments/assets/8ea7b18f-755b-44d1-83be-afe84d82690d)

U dijelu 'All Employees' se nalaze svi zaposleni. 
Selekcijom na jednog zaposlenog prikazaće se njegovi detalji u dijelu 'Employee Details'. Menadžeru su na raspolaganju opcije da izmjeni detalje zaposlenog kao što su ime, prezime, email, uloga i aktivnost.
Klikom na dugme 'Save changes' izmjenjeni podaci će biti sačuvani.

U Tab-u 'Add Employee' menadžer dodaje novog zaposlenog.

![manager window  - add employee tab](https://github.com/user-attachments/assets/c477a0f9-92a2-4a93-b891-77201b9626ee)

Menadžer popunjava podatke zaposlenog, ime, prezime, email, uloga, aktivnost i lozinka. Klikom na dugme 'Add', menadžer dodaje novog zaposlenog.
Svi podaci moraju da se popune u suprotnom će se prikazati poruka da se nisu sva polja popunila.

U Tab-u 'Add Products' se nalaze opcije za dodavanje novog proizvoda i nove kategorije.

![manager window  - add products tab](https://github.com/user-attachments/assets/9fab2b19-23a7-45a9-9fb3-1b7f38e25e46)

Menadžer popunjava podatke za novi proizvod, naziv, cijena, opis i kategorija. Klikom na dugme 'Add Product', menadžer dodaje novi proizvod sa trenutnom količinom 0.
Svi podaci moraju da se popune u suprotnom će se prikazati poruka da se nisu sva polja popunila.

Isto važi i za dodavanje nove kategorije.

U Tab-u 'Order From A Supplier' menadžer kreira nabavku.

![manager window  - order from a supplier tab](https://github.com/user-attachments/assets/281361bd-c2c4-4983-8e02-9de2cfcc9a13)

U dijelu 'All Products' nalaze se svi proizvodi koji mogu da se dodaju u nabavku. Proizvodi mogu da se pretražuju po njihovom nazivu.

Kada menadžer želi da doda proizvod u nabavku, pored detalja proizvoda postoji dugme '+' koje podrazumijevano dodaje 1 proizvod u nabavku. Klikom više puta na dugme '+' za isti proizvod, povećava se količina proizvoda u nabavci. Ako menadžer želi da doda više proizvoda odjednom, pored dugmeta '+' može da promijeni podrazumijevanu količinu u željenu.

U dijelu 'Products For Order' se nalaze proizvodi dodati u nabavku.

![manager window  - order from a supplier tab 2](https://github.com/user-attachments/assets/d6887bbd-cd58-4ee3-8578-98019819f418)

Menadžer može da ukloni proizvod iz nabavke klikom da dugme '-' koje se nalazi pored proizovda u nabavci.

U dijelu 'Order Details' se bira dobavljač. Takođe, nalaze se informacije o datumu izdavanja i ukupnom iznosu nabavke.

![manager window  - order from a supplier tab 3](https://github.com/user-attachments/assets/cc87ae8d-fcce-454c-9086-83aa99dd44dc)

Kada menadžer želi da sačuva nabavku, to vrši klikom na dugme 'Save Order' i prikaže se poruka o uspješno sačuvanoj nabavci.
Obavezno je da se odabere dobavljač kao i da postoje proizvodi u nabavci kako bi se nabavka sačuvala, u suprotnom prikazaće se greška da se nisu sva polja popunila.

U Tab-u 'View All Orders' su prikazane sve nabavke.

![manager window  - all orders tab](https://github.com/user-attachments/assets/5dc73244-d6c1-45ce-80de-0b5e844a2d37)

U dijelu 'All Orders' se nalaze sve nabavke sa njivovim brojem i datumom izdavanja.

Selekcijom jedne od nabavke, u dijelu 'Order Details' prikazaće se detalji selektovane fakture.

![manager window  - all orders tab 2](https://github.com/user-attachments/assets/50a54313-9287-48f9-b63e-f653d38c7f9d)

Detalji koji su prikazani su: detalji menadžera koji je kreirao nabavku, detalji dostavljača i svi proizvodi koji se nalaze u nabavci.

U dijelu 'Settings' se nalaze podešavanja koja su na raspolaganju menadžeru, a to je promjena jezika u englesi ili srpski. Promjena teme nije implementirana!


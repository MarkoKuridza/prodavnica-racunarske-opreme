using Projekat_A_Prodavnica_racunarske_opreme.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat_A_Prodavnica_racunarske_opreme
{
    /// <summary>
    /// Interaction logic for SellerWindow.xaml
    /// </summary>
    public partial class SellerWindow : Window, ILocalizable
    {
        public ObservableCollection<object> products { get; set; } = new ObservableCollection<object>();
        public ICollectionView FilteredProducts { get; set; }
        public ObservableCollection<proizvod> allProductsForInvoice { get; set; } = new ObservableCollection<proizvod>();
        public ICollectionView AllProductsForInvoiceView { get; set; }
        public ObservableCollection<proizvod> invoiceItems { get; set; } = new ObservableCollection<proizvod>();
        public ICollectionView InvoiceItemsView { get; set; }
        public ObservableCollection<faktura> allInvoices { get; set; } = new ObservableCollection<faktura>();
        public ICollectionView AllInvoicesView { get; set; }
        public ObservableCollection<dynamic> invoiceDetails { get; set; } = new ObservableCollection<dynamic>();
        public ICollectionView InvoiceDetailsView { get; set; }

        public SellerWindow()
        {
            InitializeComponent();
            //products tab
            LoadProductsTab();
            //create tabs
            dpDate.SelectedDate = DateTime.Now;
            dpDate.IsEnabled = false;
            LoadCreateInvoiceTab();
            LoadViewAllInvoicesTab();
            DataContext = this; //ovo mora za binding
            ApplyInternationalization();
        }

        //products tab
        private void LoadProductsTab()
        {
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                var TempProducts = (
                    from p in store.proizvods
                    join k in store.kategorija_proizvoda
                    on p.IdKategorije equals k.IdKategorije into kategorijaGroup
                    from k in kategorijaGroup.DefaultIfEmpty()
                    select new
                    {
                        p.IdProizvoda,
                        p.Naziv,
                        NazivKategorije = k.Naziv,
                        p.Kolicina,
                        p.Cijena,
                        p.Opis
                    }
                    ).ToList();

                products.Clear();
                foreach (var product in TempProducts)
                {
                    dynamic expandoProduct = new ExpandoObject();
                    expandoProduct.ProductId = product.IdProizvoda;
                    expandoProduct.PName = product.Naziv;
                    expandoProduct.Category = product.NazivKategorije;
                    expandoProduct.Quantity = product.Kolicina;
                    expandoProduct.Price = product.Cijena;
                    expandoProduct.Description = product.Opis;

                    products.Add(expandoProduct);
                }
                FilteredProducts = CollectionViewSource.GetDefaultView(products);
                FilteredProducts.Filter = FilterProducts;
                
                ProductsDetails.ItemsSource = FilteredProducts;
            }
        }

        private bool FilterProducts(object item)
        {
            if(item is IDictionary<string, object> p)
            {
                bool filterById = string.IsNullOrWhiteSpace(txtFilterId.Text) || p["ProductId"].ToString().Contains(txtFilterId.Text);
                bool filterByName = string.IsNullOrWhiteSpace(txtFilterName.Text) || p["PName"].ToString().IndexOf(txtFilterName.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                bool filterByCategory = string.IsNullOrWhiteSpace(txtFilterCategory.Text) || p["Category"].ToString().IndexOf(txtFilterCategory.Text, StringComparison.OrdinalIgnoreCase) >= 0;

                return filterById && filterByName && filterByCategory;
            }
            return false;

        }

        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            FilteredProducts.Refresh();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtFilterId.Text = "";
            txtFilterName.Text = "";
            txtFilterCategory.Text = "";
            FilteredProducts.Refresh();
        }

        //create invoice tab
        public void LoadCreateInvoiceTab()
        {
            allProductsForInvoice.Clear();
            invoiceItems.Clear();
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                foreach (var p in store.proizvods.ToList())
                {
                    allProductsForInvoice.Add(p);
                }
            }

            AllProductsForInvoiceView = CollectionViewSource.GetDefaultView(allProductsForInvoice);
            AllProductsForInvoiceView.Refresh();

            InvoiceItemsView = CollectionViewSource.GetDefaultView(invoiceItems);
            InvoiceItemsView.Refresh();

            dgAvaibleProducts.ItemsSource = AllProductsForInvoiceView;
            dgInvoiceItems.ItemsSource = InvoiceItemsView;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = txtSearch.Text.ToLower();

            AllProductsForInvoiceView.Filter = obj =>
            {
                if (obj is proizvod p)
                {
                    return string.IsNullOrWhiteSpace(text) || p.Naziv.ToLower().Contains(text);
                }
                return false;
            };

            AllProductsForInvoiceView.Refresh();
            //dgAvaibleProducts.Items.Refresh();
        }

        private void BtnAddOne_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var row = DataGridRow.GetRowContainingElement(button);
            var txtKolicina = FindVisualChild<TextBox>(row, "tbQuantity");


            if (int.TryParse(txtKolicina.Text, out int kolicina) && kolicina > 0)
            {
                var item = (proizvod)((Button)sender).DataContext;
                if (kolicina > item.Kolicina)
                { //Nema dovoljno proizvoda na stanju! Dostupno : 
                    MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerErrorMsg1", CultureInfo.CurrentCulture) + $"{item.Kolicina}");
                    return;
                }

                var itemExist = invoiceItems.FirstOrDefault(s => s.IdProizvoda == item.IdProizvoda);
                if (itemExist != null)
                {
                    itemExist.Kolicina += kolicina;
                }

                else
                {
                    invoiceItems.Add(new proizvod(item.IdProizvoda, item.Naziv, kolicina, item.Cijena));
                }
                //InvoiceItemsView.Refresh();

                item.Kolicina -= kolicina;
                decimal total = 0;
                foreach (proizvod p in invoiceItems)
                {
                    total += p.Cijena * p.Kolicina;
                }
                txtTotal.Text = total.ToString();
                dgInvoiceItems.ItemsSource = null;
                dgInvoiceItems.ItemsSource = invoiceItems;
                dgAvaibleProducts.Items.Refresh();
            }
            else
            {//Vazeca kolicina proizvoda
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerErrorMsg2", CultureInfo.CurrentCulture));
            }
        }

        //pomocna metoda
        private T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result && (result as FrameworkElement)?.Name == childName)
                    return result;

                var descendant = FindVisualChild<T>(child, childName);
                if (descendant != null)
                    return descendant;
            }
            return null;
        }

        

        private void BtnRemoveOne_Click(object sender, RoutedEventArgs e)
        {
            var itemRemoved = (proizvod)((Button)sender).DataContext;
            //Da li zelite ukloniti stavku + Potvrda uklanjanjа
            var result = MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgRemove1", CultureInfo.CurrentCulture) + $"'{itemRemoved.Naziv}'?",
                LanguageController.Instance.ResourceManager.GetString("SellerMsgRemove2", CultureInfo.CurrentCulture), MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                invoiceItems.Remove(itemRemoved);
                var temp = allProductsForInvoice.FirstOrDefault(p => itemRemoved.IdProizvoda == p.IdProizvoda);

                if (temp != null)
                {
                    temp.Kolicina += itemRemoved.Kolicina;
                }

                decimal total = 0;
                foreach (proizvod p in invoiceItems)
                {
                    total += p.Cijena * p.Kolicina;
                }
                txtTotal.Text = total.ToString();
                dgInvoiceItems.ItemsSource = null;
                dgInvoiceItems.ItemsSource = invoiceItems;
                dgAvaibleProducts.Items.Refresh();
            } 
        }

        private void BtnSaveInvoice_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtSurrname.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text)
                )
            {//Popunite sva obavezna polja!
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            try
            {
                using (ProdavnicaDb store = new ProdavnicaDb())
                {
                    using (var transaction = store.Database.BeginTransaction())
                    {
                        try {

                            var customer = new kupac()
                            {
                                Ime = txtName.Text.Trim(),
                                Prezime = txtSurrname.Text.Trim(),
                                Telefon = txtPhone.Text.Trim()
                            };

                            store.kupacs.Add(customer);
                            store.SaveChanges();

                            var invoice = new faktura()
                            {
                                Datum_Izdavanja = dpDate.SelectedDate.Value,
                                Ukupan_Iznos = decimal.Parse(txtTotal.Text.Trim()),
                                IdKupca = customer.IdKupca,
                                IdProdavca = LogInWindow.current.IdZaposlenog
                            };
                            store.fakturas.Add(invoice);
                            store.SaveChanges();

                            var items = invoiceItems.Select(p => new stavka_fakture()
                            {
                                BrojFakture = invoice.BrojFakture,
                                IdProizvoda = p.IdProizvoda,
                                Kolicina = p.Kolicina,
                                Iznos = p.Cijena * p.Kolicina
                            }).ToList();

                            store.stavka_fakture.AddRange(items);
                            store.SaveChanges();


                            transaction.Commit();
                            //Uspjeno kreirana faktura!
                            MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgNotification", CultureInfo.CurrentCulture));


                            //update kolicine u bazi
                            foreach(var item in items)
                            {
                                var product = store.proizvods.FirstOrDefault(p => p.IdProizvoda == item.IdProizvoda);
                                    if(product != null)
                                {
                                    product.Kolicina -= item.Kolicina;

                                    if (product.Kolicina < 0)
                                    {
                                        transaction.Rollback();
                                        return;
                                    }
                                }
                            }
                            store.SaveChanges();

                            //Obrisati sva txt polja, kao i invoiceItems!

                            invoiceItems.Clear();
                            txtName.Clear();
                            txtSurrname.Clear();
                            txtPhone.Clear();
                            txtTotal.Clear();
                        } catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.InnerException}");
                            transaction.Rollback();
                        }
                    }
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
            //refresh podataka
            LoadProductsTab();
            LoadCreateInvoiceTab();
            LoadViewAllInvoicesTab();
        }

        //view all invoices tab

        private void LoadViewAllInvoicesTab()
        {
            allInvoices.Clear();
            using(ProdavnicaDb store = new ProdavnicaDb())
            {
                foreach(var f in store.fakturas.ToList())
                {
                    allInvoices.Add(f);
                }
            }

            AllInvoicesView = CollectionViewSource.GetDefaultView(allInvoices);
            AllInvoicesView.Refresh();
            dgInvoices.ItemsSource = AllInvoicesView;
        }

        private void DgInvoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgInvoices.SelectedItem is faktura selectedInvoice)
            {
                using (ProdavnicaDb store = new ProdavnicaDb())
                {
                    var fullInvoice = store.fakturas
                                    .Include("kupac")
                                    .Include("zaposleni")
                                    .Include("stavka_fakture.proizvod")
                                    .Include("stavka_fakture.proizvod.kategorija_proizvoda")
                            .FirstOrDefault(f => f.BrojFakture == selectedInvoice.BrojFakture);

                    if(fullInvoice != null)
                    {
                        txtCName.Text = fullInvoice.kupac?.Ime;
                        txtCSurrname.Text = fullInvoice.kupac?.Prezime;
                        txtCPhone.Text = fullInvoice.kupac?.Telefon;

                        txtSName.Text = fullInvoice.zaposleni?.Ime;
                        txtSSurrname.Text = fullInvoice.zaposleni?.Prezime;

                        invoiceDetails.Clear();
                        foreach (var sf in fullInvoice.stavka_fakture)
                        {
                            dynamic expandoItem = new ExpandoObject();
                            expandoItem.IdProizvoda = sf.IdProizvoda;
                            expandoItem.Naziv = sf.proizvod.Naziv;
                            expandoItem.Kategorija = sf.proizvod.kategorija_proizvoda.Naziv;
                            expandoItem.Kolicina = sf.Kolicina;
                            expandoItem.Cijena = sf.proizvod.Cijena;
                            expandoItem.Iznos = sf.Iznos;

                            invoiceDetails.Add(expandoItem);
                        }

                        txtSum.Text = "Sum: " + fullInvoice.Ukupan_Iznos.ToString("N2") + " KM";
                    }
                }
            }

            InvoiceDetailsView = CollectionViewSource.GetDefaultView(invoiceDetails);
            dgItemsDetails.ItemsSource = InvoiceDetailsView;
            InvoiceDetailsView.Refresh();
        }


        //settings tab
        private void BtnEnglish_Click(object sender, RoutedEventArgs e)
        {
            LanguageController.Instance.ChangeLanguage("en");
        }

        private void BtnSerbian_Click(object sender, RoutedEventArgs e)
        {
            LanguageController.Instance.ChangeLanguage("sr");

        }

        private void BtnLight_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnDark_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnSilver_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //languages
        //pomocna metoda
        public string GetFormatted(string key, params object[] args)
        {
            var raw = LanguageController.Instance.ResourceManager.GetString(key, CultureInfo.CurrentCulture);
            return string.Format(raw, args);
        }
        public void ApplyInternationalization()
        {
            Title = GetFormatted("Title_SellerWindow", $"{LogInWindow.current.Ime} {LogInWindow.current.Prezime}");
            Products.Header = LanguageController.Instance.ResourceManager.GetString("Products", CultureInfo.CurrentCulture);
            txtFilterId.Watermark = LanguageController.Instance.ResourceManager.GetString("txtFilterId", CultureInfo.CurrentCulture);
            txtFilterName.Watermark = LanguageController.Instance.ResourceManager.GetString("txtFilterName", CultureInfo.CurrentCulture);
            txtFilterCategory.Watermark = LanguageController.Instance.ResourceManager.GetString("txtFilterCategory", CultureInfo.CurrentCulture);
            btnFilter.Content = LanguageController.Instance.ResourceManager.GetString("btnFilter", CultureInfo.CurrentCulture);
            btnClear.Content = LanguageController.Instance.ResourceManager.GetString("btnClear", CultureInfo.CurrentCulture);
            ProductId.Header = LanguageController.Instance.ResourceManager.GetString("ProductId", CultureInfo.CurrentCulture);
            Name.Header = LanguageController.Instance.ResourceManager.GetString("Name", CultureInfo.CurrentCulture);
            Category.Header = LanguageController.Instance.ResourceManager.GetString("Category", CultureInfo.CurrentCulture);
            Quantity.Header = LanguageController.Instance.ResourceManager.GetString("Quantity", CultureInfo.CurrentCulture);
            Price.Header = LanguageController.Instance.ResourceManager.GetString("Price", CultureInfo.CurrentCulture);
            Description.Header = LanguageController.Instance.ResourceManager.GetString("Description", CultureInfo.CurrentCulture);
            CInvoice.Header = LanguageController.Instance.ResourceManager.GetString("CInvoice", CultureInfo.CurrentCulture);
            gpInvoiceD.Header = LanguageController.Instance.ResourceManager.GetString("gpInvoiceD", CultureInfo.CurrentCulture);
            txtName.Watermark = LanguageController.Instance.ResourceManager.GetString("txtName", CultureInfo.CurrentCulture);
            txtSurrname.Watermark = LanguageController.Instance.ResourceManager.GetString("txtSurrname", CultureInfo.CurrentCulture);
            txtPhone.Watermark = LanguageController.Instance.ResourceManager.GetString("txtPhone", CultureInfo.CurrentCulture);
            lbCustomer.Content = LanguageController.Instance.ResourceManager.GetString("lbCustomer", CultureInfo.CurrentCulture);
            lbIssueDate.Content = LanguageController.Instance.ResourceManager.GetString("lbIssueDate", CultureInfo.CurrentCulture);
            lbTotal.Content = LanguageController.Instance.ResourceManager.GetString("lbTotal", CultureInfo.CurrentCulture);
            btnSave.Content = LanguageController.Instance.ResourceManager.GetString("btnSave", CultureInfo.CurrentCulture);
            gpInvoiceI.Header = LanguageController.Instance.ResourceManager.GetString("gpInvoiceI", CultureInfo.CurrentCulture);
            IName.Header = LanguageController.Instance.ResourceManager.GetString("IName", CultureInfo.CurrentCulture);
            IQuantity.Header = LanguageController.Instance.ResourceManager.GetString("IQuantity", CultureInfo.CurrentCulture);
            IPrice.Header = LanguageController.Instance.ResourceManager.GetString("IPrice", CultureInfo.CurrentCulture);
            gpAvaibleP.Header = LanguageController.Instance.ResourceManager.GetString("gpAvaibleP", CultureInfo.CurrentCulture);
            txtSearch.Watermark = LanguageController.Instance.ResourceManager.GetString("txtSearch", CultureInfo.CurrentCulture);
            AName.Header = LanguageController.Instance.ResourceManager.GetString("IName", CultureInfo.CurrentCulture);
            AQuantity.Header = LanguageController.Instance.ResourceManager.GetString("IQuantity", CultureInfo.CurrentCulture);
            APrice.Header = LanguageController.Instance.ResourceManager.GetString("IPrice", CultureInfo.CurrentCulture);
            ViewInvoice.Header = LanguageController.Instance.ResourceManager.GetString("ViewInvoice", CultureInfo.CurrentCulture);
            gpInvoices.Header = LanguageController.Instance.ResourceManager.GetString("gpInvoices", CultureInfo.CurrentCulture);
            InvoiceNumber.Header = LanguageController.Instance.ResourceManager.GetString("InvoiceNumber", CultureInfo.CurrentCulture);
            Date.Header = LanguageController.Instance.ResourceManager.GetString("Date", CultureInfo.CurrentCulture);
            gpInvoiceDetails.Header = LanguageController.Instance.ResourceManager.GetString("gpInvoiceD", CultureInfo.CurrentCulture);
            lbCustomerInvoiceD.Content = LanguageController.Instance.ResourceManager.GetString("lbCustomer", CultureInfo.CurrentCulture);
            lbSeller.Content = LanguageController.Instance.ResourceManager.GetString("lbSeller", CultureInfo.CurrentCulture);
            ProductIdI.Header = LanguageController.Instance.ResourceManager.GetString("ProductId", CultureInfo.CurrentCulture);
            NameI.Header = LanguageController.Instance.ResourceManager.GetString("Name", CultureInfo.CurrentCulture);
            CategoryI.Header = LanguageController.Instance.ResourceManager.GetString("Category", CultureInfo.CurrentCulture);
            QuantityI.Header = LanguageController.Instance.ResourceManager.GetString("Quantity", CultureInfo.CurrentCulture);
            PriceI.Header = LanguageController.Instance.ResourceManager.GetString("Price", CultureInfo.CurrentCulture);
            AmountI.Header = LanguageController.Instance.ResourceManager.GetString("Amount", CultureInfo.CurrentCulture);
            Settings.Header = LanguageController.Instance.ResourceManager.GetString("Settings", CultureInfo.CurrentCulture);
            Language.Content = LanguageController.Instance.ResourceManager.GetString("Language", CultureInfo.CurrentCulture);
            Theme.Content = LanguageController.Instance.ResourceManager.GetString("Theme", CultureInfo.CurrentCulture);
            BtnEnglish.Content = LanguageController.Instance.ResourceManager.GetString("MenuLangEn", CultureInfo.CurrentCulture);
            BtnSerbian.Content = LanguageController.Instance.ResourceManager.GetString("MenuLangSr", CultureInfo.CurrentCulture);
            BtnLight.Content = LanguageController.Instance.ResourceManager.GetString("BtnLight", CultureInfo.CurrentCulture);
            BtnDark.Content = LanguageController.Instance.ResourceManager.GetString("BtnDark", CultureInfo.CurrentCulture);
            BtnSilver.Content = LanguageController.Instance.ResourceManager.GetString("BtnSilver", CultureInfo.CurrentCulture);
            txtProductsData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtAvaibleProductsData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtInvoiceData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtInvoicesData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtInvoiceDetailsData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
        }

    }
}

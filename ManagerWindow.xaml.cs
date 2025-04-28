using Projekat_A_Prodavnica_racunarske_opreme.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekat_A_Prodavnica_racunarske_opreme
{
    /// <summary>
    /// Interaction logic for ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window, ILocalizable
    {
        public List<zaposleni> employees;
        public List<proizvod> allProducts;
        public List<proizvod> order;
        public List<nabavka> allOrders;

        public ManagerWindow()
        {
            InitializeComponent();
            LoadEditEmployeeTab();
            dpDateAdd.SelectedDate = DateTime.Now;
            dpDateAdd.IsEnabled = false;
            dpDate.SelectedDate = DateTime.Now;
            dpDate.IsEnabled = false;
            LoadOrderTab();
            LoadViewAllOrdersTab();
            ApplyInternationalization();
        }

        //Employees tab

        public void LoadEditEmployeeTab()
        {
            employees = new List<zaposleni>();
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                employees = store.zaposlenis.ToList();
            }
            dgEmployees.ItemsSource = null;
            dgEmployees.ItemsSource = employees;
            var roles = new List<string> { "Menadzer", "Prodavac" };

            cbRole.ItemsSource = roles;
            //ovo je za tab za dodavanje zaposlenog
            cbRoleAdd.ItemsSource = roles;
            //za tab za dodavanje proizvoda
            LoadCategoryCB();
        }

        private void DgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEmployees.SelectedItem is zaposleni selectedEmployee)
            {
                txtId.Text = selectedEmployee.IdZaposlenog.ToString();
                txtName.Text = selectedEmployee.Ime;
                txtSurrname.Text = selectedEmployee.Prezime;
                txtEmail.Text = selectedEmployee.Email;
                cbRole.SelectedItem = selectedEmployee.Uloga;
                tbDate.Text = selectedEmployee.Datum_zaposlenja.ToString("d");
                if (selectedEmployee.Aktivan == 1)
                {
                    cbActive.IsChecked = true;
                }
                else
                {
                    cbActive.IsChecked = false;
                }
            }
        }

        private void BtnSaveChanges_Click(object sender, RoutedEventArgs e)
        {   
            if(string.IsNullOrWhiteSpace(txtName.Text)||
                string.IsNullOrWhiteSpace(txtSurrname.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text))
            {
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                using (var transaction = store.Database.BeginTransaction())
                {
                    try
                    {
                        int temp = int.Parse(txtId.Text.Trim());
                        var edit = store.zaposlenis.FirstOrDefault(z => z.IdZaposlenog == temp);
                        if (edit != null)
                        {
                            edit.Ime = txtName.Text.Trim();
                            edit.Prezime = txtSurrname.Text.Trim();
                            edit.Email = txtEmail.Text.Trim();
                            edit.Uloga = cbRole.Text.Trim();
                            edit.Aktivan = (sbyte)(cbActive.IsChecked == true ? 1 : 0);
                        }
                        store.SaveChanges();
                        transaction.Commit();
                        //Uspjesno izvrsena izmjena zaposlenog:
                        MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("ManagerMsgNotification1", CultureInfo.CurrentCulture) + $"{edit.Ime} {edit.Prezime}");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.InnerException}");
                        transaction.Rollback();
                    }
                }
                LoadEditEmployeeTab();
            }
        }

        //Add Employee tab
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNameAdd.Text) ||
                string.IsNullOrWhiteSpace(txtSurrnameAdd.Text) ||
                string.IsNullOrWhiteSpace(txtEmailAdd.Text) ||
                string.IsNullOrWhiteSpace(cbRole.Text) ||
                string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                using (var transaction = store.Database.BeginTransaction())
                {
                    try
                    {
                        var newEmployee = new zaposleni()
                        {
                            Ime = txtNameAdd.Text.Trim(),
                            Prezime = txtSurrnameAdd.Text.Trim(),
                            Email = txtEmailAdd.Text.Trim(),
                            Uloga = cbRoleAdd.Text.Trim(),
                            Datum_zaposlenja = dpDateAdd.SelectedDate.Value,
                            Aktivan = (sbyte)(cbActiveAdd.IsChecked == true ? 1 : 0),
                            Lozinka = Util.PasswordHash.HashPassword(txtPass.Text.Trim())
                        };
                        store.zaposlenis.Add(newEmployee);
                        store.SaveChanges();
                        transaction.Commit();

                        //Uspjesno dodat novi zaposleni:
                        MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("ManagerMsgNotification", CultureInfo.CurrentCulture) + $"{newEmployee.Ime} {newEmployee.Prezime}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ex.InnerException}");
                        transaction.Rollback();
                    }
                    txtNameAdd.Clear();
                    txtSurrnameAdd.Clear();
                    txtEmailAdd.Clear();
                    cbRoleAdd.ItemsSource = null;
                    cbActiveAdd.IsChecked = false;
                    txtPass.Clear();

                }
            }
            LoadEditEmployeeTab();
        }

        //add product tab
        private void LoadCategoryCB()
        {
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                var cat = store.kategorija_proizvoda.ToList();
                List<string> catName = new List<string>();
                foreach (kategorija_proizvoda kp in cat)
                {
                    catName.Add(kp.Naziv);
                }
                cbCategoryP.ItemsSource = catName;
            }
        }

        private void TxtPriceP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d*\.?\d{0,2}$");
            e.Handled = !regex.IsMatch(((TextBox)sender).Text.Trim());
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNameP.Text) ||
                string.IsNullOrWhiteSpace(txtPriceP.Text) ||
                string.IsNullOrWhiteSpace(txtDescriptionP.Text) ||
                string.IsNullOrWhiteSpace(cbCategoryP.Text))
            {
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            try
            {
                using (ProdavnicaDb store = new ProdavnicaDb())
                {
                    using (var transaction = store.Database.BeginTransaction())
                    {
                        try
                        {
                            var temp = store.kategorija_proizvoda.FirstOrDefault(kp => kp.Naziv == cbCategoryP.Text.Trim());

                            var newProduct = new proizvod()
                            {
                                Naziv = txtNameP.Text.Trim(),
                                Kolicina = 0,
                                Cijena = decimal.Parse(txtPriceP.Text.Trim()),
                                Opis = txtDescriptionP.Text.Trim(),
                                IdKategorije = temp.IdKategorije
                            };

                            store.proizvods.Add(newProduct);
                            store.SaveChanges();
                            transaction.Commit();

                            //Uspjesno dodat novi proizvod
                            MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("ManagerMsgNotification2", CultureInfo.CurrentCulture));

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.InnerException}");
                            transaction.Rollback();
                        }
                        txtNameP.Clear();
                        txtPriceP.Clear();
                        txtDescriptionP.Clear();
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"{ex.InnerException}");
            }
            
            LoadOrderTab();
        }

        private void BtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtCatName.Text) ||
                string.IsNullOrWhiteSpace(txtDescriptionC.Text))
            {
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            try
            {
                using (ProdavnicaDb store = new ProdavnicaDb())
                {
                    using (var transaction = store.Database.BeginTransaction())
                    {
                        try
                        {
                            var newCategory = new kategorija_proizvoda()
                            {
                                Naziv = txtCatName.Text.Trim(),
                                Opis = txtDescriptionC.Text.Trim()
                            };
                            store.kategorija_proizvoda.Add(newCategory);
                            store.SaveChanges();
                            transaction.Commit();

                            //Uspjesno dodata nova kategorija
                            MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("ManagerMsgNotification3", CultureInfo.CurrentCulture));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"{ex.InnerException}");
                            transaction.Rollback();
                        }
                        txtCatName.Clear();
                        txtDescriptionC.Clear();
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
            LoadCategoryCB();
        }

        //order from a supplier tab
        public void LoadOrderTab()
        {
            using (ProdavnicaDb store = new ProdavnicaDb())
            {
                var productsInvoice = store.proizvods.ToList();
                //ovako funcionise
                allProducts = new List<proizvod>();
                foreach (var p in productsInvoice)
                {
                    allProducts.Add(new proizvod(p.IdProizvoda, p.Naziv, p.Kolicina, p.Cijena));
                }
                dgAllP.ItemsSource = null;
                dgAllP.ItemsSource = allProducts;

                //za dobavljace
                var suppliers = store.dobavljacs.ToList();
                List<string> suppName = new List<string>();
                foreach(dobavljac d in suppliers)
                {
                    suppName.Add(d.Naziv);
                }
                cbSuppliers.ItemsSource = suppName;               
            }
            //stavke nabavke
            order = new List<proizvod>();
            dgOrderProducts.ItemsSource = null;
            dgOrderProducts.ItemsSource = order;
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var text = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(text))
            {
                dgAllP.ItemsSource = allProducts;
            }
            else
            {
                var filter = allProducts.Where(p => p.Naziv.ToLower().Contains(text)).ToList();
                dgAllP.ItemsSource = filter;
            }
        }

        private void BtnAddOneProduct_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var row = DataGridRow.GetRowContainingElement(button);
            var txtKolicina = FindVisualChild<TextBox>(row, "tbQuantity");

            if (int.TryParse(txtKolicina.Text, out int kolicina))
            {
                var product = (proizvod)((Button)sender).DataContext;

                var productExist = order.FirstOrDefault(s => s.IdProizvoda == product.IdProizvoda);
                if (productExist != null)
                {
                    productExist.Kolicina += kolicina;
                }
                else
                {
                    order.Add(new proizvod(product.IdProizvoda, product.Naziv, kolicina, product.Cijena));
                }
                
                decimal total = 0;
                foreach (proizvod p in order)
                {
                    total += p.Cijena * p.Kolicina;
                }
                txtTotal.Text = total.ToString();
                dgOrderProducts.ItemsSource = null;
                dgOrderProducts.ItemsSource = order;
                dgAllP.Items.Refresh();
            }
            else
            {
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

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            var itemRemoved = (proizvod)((Button)sender).DataContext;
            //Da li zelite ukloniti stavku + Potvrda uklanjanjа
            var result = MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgRemove1", CultureInfo.CurrentCulture) + $"'{itemRemoved.Naziv}'?",
                LanguageController.Instance.ResourceManager.GetString("SellerMsgRemove2", CultureInfo.CurrentCulture), MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                order.Remove(itemRemoved);
                
                decimal total = 0;
                foreach (proizvod p in order)
                {
                    total += p.Cijena * p.Kolicina;
                }
                txtTotal.Text = total.ToString();
                dgOrderProducts.ItemsSource = null;
                dgOrderProducts.ItemsSource = order;
                dgAllP.Items.Refresh();
            }

        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if(order.Count == 0 ||
                string.IsNullOrWhiteSpace(cbSuppliers.Text))
            {
                //nema proizvoda za naruciti
                MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("SellerMsgWarning1", CultureInfo.CurrentCulture));
                return;
            }
            using(ProdavnicaDb store = new ProdavnicaDb())
            {
                using(var transaction = store.Database.BeginTransaction())
                {
                    try
                    {
                        var temp = store.dobavljacs.FirstOrDefault(d => d.Naziv == cbSuppliers.Text);
                        var newOrder = new nabavka()
                        {
                            Datum_Nabavke = dpDate.SelectedDate.Value,
                            Ukupan_Iznos = decimal.Parse(txtTotal.Text.Trim()),
                            IdMenadzera = LogInWindow.current.IdZaposlenog,
                            IdDobavljaca = temp.IdDobavljaca
                        };
                        store.nabavkas.Add(newOrder);
                        store.SaveChanges();

                        var orderProducts = order.Select(p => new stavka_nabavke()
                        {
                            IdProizvoda = p.IdProizvoda,
                            IdNabavke = newOrder.IdNabavke,
                            Kolicina = p.Kolicina,
                            Iznos = p.Cijena * p.Kolicina
                        }).ToList();
                        store.stavka_nabavke.AddRange(orderProducts);
                        store.SaveChanges();
                    
                        transaction.Commit();
                        //poruka uspjesne nabavke
                        MessageBox.Show(LanguageController.Instance.ResourceManager.GetString("ManagerMsgNotification4", CultureInfo.CurrentCulture));
                        
                        //update kolicine proizvoda
                        foreach (var product in orderProducts)
                        {
                            var item = store.proizvods.First(p => p.IdProizvoda == product.IdProizvoda);
                            if(item != null)
                            {
                                item.Kolicina += product.Kolicina;
                            }
                        }
                        store.SaveChanges();

                    }catch(Exception ex)
                    {
                        MessageBox.Show($"{ex.InnerException}");
                        transaction.Rollback();
                    }
                    order.Clear();
                    txtTotal.Clear();
                }
            }
            LoadOrderTab();
            LoadViewAllOrdersTab();
        }

        //view all orders tab

        public void LoadViewAllOrdersTab()
        {
            allOrders = new List<nabavka>();
            using(ProdavnicaDb store = new ProdavnicaDb())
            {
                allOrders = store.nabavkas.ToList();

                dgOrders.ItemsSource = null;
                dgOrders.ItemsSource = allOrders;
            }
        }
        
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgOrders.SelectedItem is nabavka selectedOrder)
            {
                dgOrdersDetails.ItemsSource = null;
                using(ProdavnicaDb store = new ProdavnicaDb())
                {
                    var fullOrder = store.nabavkas
                        .Include("zaposleni")
                        .Include("dobavljac")
                        .Include("stavka_nabavke.proizvod")
                        .Include("stavka_nabavke.proizvod.kategorija_proizvoda")
                        .FirstOrDefault(o => o.IdNabavke == selectedOrder.IdNabavke);
                    if (fullOrder != null)
                    {
                        txtMName.Text = fullOrder.zaposleni?.Ime;
                        txtMSurrname.Text = fullOrder.zaposleni?.Prezime;
                        txtSName.Text = fullOrder.dobavljac?.Naziv;
                        txtSEmail.Text = fullOrder.dobavljac?.Email;
                    }

                    var itemsDisplay = fullOrder.stavka_nabavke.Select(sn => new {
                        IdProizvoda = sn.IdProizvoda,
                        Naziv = sn.proizvod.Naziv,
                        Kategorija = sn.proizvod.kategorija_proizvoda.Naziv,
                        Kolicina = sn.Kolicina,
                        Cijena = sn.proizvod.Cijena,
                        Iznos = sn.Iznos
                    }).ToList();

                    dgOrdersDetails.ItemsSource = itemsDisplay;

                    txtSum.Text = "Sum: " + fullOrder.Ukupan_Iznos.ToString("N2") + " KM";
                }
            }
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
            Title = GetFormatted("Title_ManagerWindow", $"{LogInWindow.current.Ime} {LogInWindow.current.Prezime}");
            EditEmployees.Header = LanguageController.Instance.ResourceManager.GetString("EditEmployee", CultureInfo.CurrentCulture);
            AllEmpolyees.Header = LanguageController.Instance.ResourceManager.GetString("AllEmployees", CultureInfo.CurrentCulture);
            NameEmp.Header = LanguageController.Instance.ResourceManager.GetString("NameEmp", CultureInfo.CurrentCulture);
            SurrnameEmp.Header = LanguageController.Instance.ResourceManager.GetString("SurrnameEmp", CultureInfo.CurrentCulture);
            EmployeeDetails.Header = LanguageController.Instance.ResourceManager.GetString("EmployeeDetails", CultureInfo.CurrentCulture);
            lbName.Content = LanguageController.Instance.ResourceManager.GetString("NameEmp", CultureInfo.CurrentCulture);
            lbSurrname.Content = LanguageController.Instance.ResourceManager.GetString("SurrnameEmp", CultureInfo.CurrentCulture);
            lbRole.Content = LanguageController.Instance.ResourceManager.GetString("lbRole", CultureInfo.CurrentCulture);
            lbDate.Content = LanguageController.Instance.ResourceManager.GetString("lbDate", CultureInfo.CurrentCulture);
            cbActive.Content = LanguageController.Instance.ResourceManager.GetString("cbActive", CultureInfo.CurrentCulture);
            btnSaveChanges.Content = LanguageController.Instance.ResourceManager.GetString("btnSaveChanges", CultureInfo.CurrentCulture);
            AddEmployee.Header = LanguageController.Instance.ResourceManager.GetString("AddEmployee", CultureInfo.CurrentCulture);
            lbNameAdd.Content = LanguageController.Instance.ResourceManager.GetString("NameEmp", CultureInfo.CurrentCulture);
            lbSurrnameAdd.Content = LanguageController.Instance.ResourceManager.GetString("SurrnameEmp", CultureInfo.CurrentCulture);
            lbRoleAdd.Content = LanguageController.Instance.ResourceManager.GetString("lbRole", CultureInfo.CurrentCulture);
            lbDateAdd.Content = LanguageController.Instance.ResourceManager.GetString("lbDate", CultureInfo.CurrentCulture);
            cbActiveAdd.Content = LanguageController.Instance.ResourceManager.GetString("cbActive", CultureInfo.CurrentCulture);
            lbPass.Content = LanguageController.Instance.ResourceManager.GetString("LbPass", CultureInfo.CurrentCulture);
            btnAdd.Content = LanguageController.Instance.ResourceManager.GetString("btnAdd", CultureInfo.CurrentCulture);
            AddProducts.Header = LanguageController.Instance.ResourceManager.GetString("AddProducts", CultureInfo.CurrentCulture);
            lbNameP.Content = LanguageController.Instance.ResourceManager.GetString("Name", CultureInfo.CurrentCulture);
            lbPriceP.Content = LanguageController.Instance.ResourceManager.GetString("Price", CultureInfo.CurrentCulture);
            lbDescriptionP.Content = LanguageController.Instance.ResourceManager.GetString("Description", CultureInfo.CurrentCulture);
            lbCategoryP.Content = LanguageController.Instance.ResourceManager.GetString("Category", CultureInfo.CurrentCulture);
            btnAddProduct.Content = LanguageController.Instance.ResourceManager.GetString("btnAddProduct", CultureInfo.CurrentCulture);
            lbNewCat.Content = LanguageController.Instance.ResourceManager.GetString("lbNewCat", CultureInfo.CurrentCulture);
            lbCatName.Content = LanguageController.Instance.ResourceManager.GetString("lbCatName", CultureInfo.CurrentCulture);
            lbDescriptionC.Content = LanguageController.Instance.ResourceManager.GetString("Description", CultureInfo.CurrentCulture);
            btnAddCategory.Content = LanguageController.Instance.ResourceManager.GetString("btnAddCategory", CultureInfo.CurrentCulture);
            Order.Header = LanguageController.Instance.ResourceManager.GetString("Order", CultureInfo.CurrentCulture);
            gpAllP.Header = LanguageController.Instance.ResourceManager.GetString("gpAllP", CultureInfo.CurrentCulture);
            txtSearch.Watermark = LanguageController.Instance.ResourceManager.GetString("txtSearch", CultureInfo.CurrentCulture);
            AName.Header = LanguageController.Instance.ResourceManager.GetString("IName", CultureInfo.CurrentCulture);
            AQuantity.Header = LanguageController.Instance.ResourceManager.GetString("IQuantity", CultureInfo.CurrentCulture);
            APrice.Header = LanguageController.Instance.ResourceManager.GetString("IPrice", CultureInfo.CurrentCulture);
            gpOrder.Header = LanguageController.Instance.ResourceManager.GetString("gpOrder", CultureInfo.CurrentCulture);
            IName.Header = LanguageController.Instance.ResourceManager.GetString("IName", CultureInfo.CurrentCulture);
            IQuantity.Header = LanguageController.Instance.ResourceManager.GetString("IQuantity", CultureInfo.CurrentCulture);
            IPrice.Header = LanguageController.Instance.ResourceManager.GetString("IPrice", CultureInfo.CurrentCulture);
            gpOrderD.Header = LanguageController.Instance.ResourceManager.GetString("gpOrderD", CultureInfo.CurrentCulture);
            lbSupplier.Content = LanguageController.Instance.ResourceManager.GetString("lbSupplier", CultureInfo.CurrentCulture);
            lbIssueDate.Content = LanguageController.Instance.ResourceManager.GetString("lbIssueDate", CultureInfo.CurrentCulture);
            lbTotal.Content = LanguageController.Instance.ResourceManager.GetString("lbTotal", CultureInfo.CurrentCulture);
            btnSaveOrder.Content = LanguageController.Instance.ResourceManager.GetString("btnSaveOrder", CultureInfo.CurrentCulture);
            ViewOrders.Header = LanguageController.Instance.ResourceManager.GetString("ViewOrders", CultureInfo.CurrentCulture);
            gpOrders.Header = LanguageController.Instance.ResourceManager.GetString("gpOrders", CultureInfo.CurrentCulture);
            OrderNumber.Header = LanguageController.Instance.ResourceManager.GetString("OrderNumber", CultureInfo.CurrentCulture);
            Date.Header = LanguageController.Instance.ResourceManager.GetString("lbIssueDate", CultureInfo.CurrentCulture);
            gpOrderDetails.Header = LanguageController.Instance.ResourceManager.GetString("gpOrderD", CultureInfo.CurrentCulture);
            lbManager.Content = LanguageController.Instance.ResourceManager.GetString("lbManager", CultureInfo.CurrentCulture);
            lbSupplierOrder.Content = LanguageController.Instance.ResourceManager.GetString("lbSupplier", CultureInfo.CurrentCulture);
            ProductIdO.Header = LanguageController.Instance.ResourceManager.GetString("ProductId", CultureInfo.CurrentCulture);
            NameO.Header = LanguageController.Instance.ResourceManager.GetString("Name", CultureInfo.CurrentCulture);
            CategoryO.Header = LanguageController.Instance.ResourceManager.GetString("Category", CultureInfo.CurrentCulture);
            QuantityO.Header = LanguageController.Instance.ResourceManager.GetString("Quantity", CultureInfo.CurrentCulture);
            PriceO.Header = LanguageController.Instance.ResourceManager.GetString("Price", CultureInfo.CurrentCulture);
            AmountO.Header = LanguageController.Instance.ResourceManager.GetString("Amount", CultureInfo.CurrentCulture);
            Settings.Header = LanguageController.Instance.ResourceManager.GetString("Settings", CultureInfo.CurrentCulture);
            Language.Content = LanguageController.Instance.ResourceManager.GetString("Language", CultureInfo.CurrentCulture);
            lbTheme.Content = LanguageController.Instance.ResourceManager.GetString("Theme", CultureInfo.CurrentCulture);
            BtnEnglish.Content = LanguageController.Instance.ResourceManager.GetString("MenuLangEn", CultureInfo.CurrentCulture);
            BtnSerbian.Content = LanguageController.Instance.ResourceManager.GetString("MenuLangSr", CultureInfo.CurrentCulture);
            BtnLight.Content = LanguageController.Instance.ResourceManager.GetString("BtnLight", CultureInfo.CurrentCulture);
            BtnDark.Content = LanguageController.Instance.ResourceManager.GetString("BtnDark", CultureInfo.CurrentCulture);
            BtnSilver.Content = LanguageController.Instance.ResourceManager.GetString("BtnSilver", CultureInfo.CurrentCulture);
            txtEmployeesData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtProductsData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtProductsOrderData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtOrdersData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
            txtOrdersItemsData.Text = LanguageController.Instance.ResourceManager.GetString("EmptyDataGrid", CultureInfo.CurrentCulture);
        }


    }
}

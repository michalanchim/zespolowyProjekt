using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Serwis
{
    public partial class Form1 : Form
    {
        //FolderBrowserDialog fbd;
        string path;

        string[] serwisanci;
        Label loginLabel, hasloLabel;
        Label[] id, imie, nazwisko, telefon, rola,loginPracownika, hasloPracownika, idSprzetu, typ, producent,
            idWlasciciela, status, koszt, dataPrzyjecia, dataOdbioru;
        RichTextBox imieE, nazwiskoE, telefonE, rolaE, loginPracownikaE, hasloPracownikaE, typE, producentE,
            idWlascicielaE, kosztE, opisE, dataPrzyjeciaE, dataOdbioruE;
        ComboBox nazwiskoSerwisanta, rolaPracownika, idWlascicielaW, statusE;
        NumericUpDown idE, idSprzetuE;
        RichTextBox[] opis;
        TextBox login, haslo;
        Button zaloguj,wyloguj,zamknij,wyswietlSerwisy,zarzadzajSerwisami,
            wyswietlKlientow,zarzadzajklientami,wyswietlSprzet,
            zarzadzajSprzetem,wyswietlPracownikow,zarzadzajPracownikami,
            Dodaj, Zmodyfikuj, Usun, Akceptuj;
        GroupBox sortKl = new GroupBox();
        pracownicy[] pracownik;
        klienci[] klient;
        sprzety[] sprzet;
        serwisy[] serwis;
        Label sortowaniePoNazwisku, sortowaniePoTelefonie;
        RichTextBox sortPoNazwisku, sortPoTelefonie;
        private bool zalogowano;
        pracownicy zalogowany = new pracownicy(0, "", "", "", "", "", true);
        private int idNowego;
        private klienci nowy = new klienci(0, "", "", "", true);
        int wielkoscBazy = 999;

        public Form1()
        {
            path = @"D:\testzapisu\";
            //path = Directory.GetCurrentDirectory() + "";
            InitializeComponent();
            budujPanelLogowania();

            statusE = new ComboBox();
            statusE.Items.Add("Do naprawy");
            statusE.Items.Add("W naprawie");
            statusE.Items.Add("Do odbioru");
            statusE.Items.Add("Odebrane");

            pracownik = new pracownicy[50];
            klient = new klienci[wielkoscBazy];
            sprzet = new sprzety[wielkoscBazy];
            serwis = new serwisy[wielkoscBazy];
            serwisanci = new string[50];
            wczytajDane();
            /*
            fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            path = fbd.SelectedPath + "\\";
            while (!Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path);
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("Brak dostępu do lokalizacji, prosze spróbować ponownie.");
                    fbd.ShowDialog();
                }
            }
            */
        }

        private void wczytajDane()
        {
            int index = 0;
            if (File.Exists(path + "pracownicy.txt"))
            {
                string[] pracownicyWD = File.ReadAllLines(path+"pracownicy.txt");
                index = 0;
                foreach (string line in pracownicyWD)
                {
                    string[] substring = line.Split(':');
                        pracownik[index] = new pracownicy(Convert.ToInt32(substring[0]), Convert.ToString(substring[1]),
                            Convert.ToString(substring[2]), Convert.ToString(substring[3]),
                            Convert.ToString(substring[4]), Convert.ToString(substring[5]),
                            Convert.ToBoolean(substring[6]));
                    
                    index++;
                }
                if (pracownik[0]==null)
                {
                    tworzenieAdministratora();
                }
            }
            else
            {
                tworzenieAdministratora();
            }
            if (File.Exists(path+"klienci.txt"))
            {
                string[] klienciWD = File.ReadAllLines(path+"klienci.txt");
                index = 0;
                foreach (string line in klienciWD)
                {
                    string[] substring = line.Split(':');
                    bool akt = (substring[4] == "True");
                    klient[index] = new klienci(Convert.ToInt32(substring[0]), Convert.ToString(substring[1]),
                        Convert.ToString(substring[2]), Convert.ToString(substring[3]), akt);
                    if (akt)
                    {
                        Console.WriteLine(substring[4] == "True");
                    }
                    index++;
                }
            }
            else if(File.Exists(path + "klienciKopia.txt"))
            {
                File.Copy(path + "klienciKopia.txt", path + "klienci.txt");
            }
            else if (File.Exists(path + "klienciBackup.txt"))
            {
                File.Copy(path + "klienciBackup.txt", path + "klienci.txt");
            }

            if (File.Exists(path+"sprzety.txt"))
            {
                string[] sprzetyWD = File.ReadAllLines(path+"sprzety.txt");
                index = 0;
                foreach (string line in sprzetyWD)
                {
                    string[] substring = line.Split(':');
                    sprzet[index] = new sprzety(Convert.ToInt32(substring[0]), Convert.ToString(substring[1]),
                        Convert.ToString(substring[2]), Convert.ToString(substring[3]), Convert.ToInt32(substring[4]), 
                        Convert.ToString(substring[5]), Convert.ToString(substring[6]), Convert.ToString(substring[7]));

                    index++;
                }
            }
            else if (File.Exists(path + "sprzetyKopia.txt"))
            {
                File.Copy(path + "sprzetyKopia.txt", path + "sprzety.txt");
            }
            else if (File.Exists(path + "sprzetyBackup.txt"))
            {
                File.Copy(path + "sprzetyBackup.txt", path + "sprzety.txt");
            }

            if (File.Exists(path+"serwisy.txt"))
            {
                string[] serwisyWD = File.ReadAllLines(path+"serwisy.txt");
                index = 0;
                foreach (string line in serwisyWD)
                {
                    string[] substring = line.Split(':');
                    serwis[index] = new serwisy(Convert.ToInt32(substring[0]), Convert.ToString(substring[1]),
                        Convert.ToString(substring[2]), Convert.ToDouble(substring[3]));

                    index++;
                }
            }
            else if (File.Exists(path + "serwisyKopia.txt"))
            {
                File.Copy(path + "serwisyKopia.txt", path + "serwisy.txt");
            }
            else if (File.Exists(path + "serwisyBackup.txt"))
            {
                File.Copy(path + "serwisyBackup.txt", path + "serwisy.txt");
            }

        }

        private void tworzenieAdministratora()
        {
            MessageBox.Show("Nie ma jeszcze administratora w bazie, prosze podać dane administratora.");
            prawy.Controls.Clear();
            id = new Label[pracownik.Length + 1];
            imie = new Label[pracownik.Length + 1];
            nazwisko = new Label[pracownik.Length + 1];
            loginPracownika = new Label[pracownik.Length + 1];
            hasloPracownika = new Label[pracownik.Length + 1];
            rola = new Label[pracownik.Length + 1];

            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(10, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            loginPracownika[0] = new Label();
            loginPracownika[0].Width = 150;
            loginPracownika[0].Height = 30;
            loginPracownika[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            loginPracownika[0].Text = "Login pracownika";
            loginPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            hasloPracownika[0] = new Label();
            hasloPracownika[0].Width = 150;
            hasloPracownika[0].Height = 30;
            hasloPracownika[0].Location = new Point(loginPracownika[0].Width + loginPracownika[0].Location.X + 5, 15);
            hasloPracownika[0].Text = "Hasło pracownika";
            hasloPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            rola[0] = new Label();
            rola[0].Width = 150;
            rola[0].Height = 30;
            rola[0].Location = new Point(hasloPracownika[0].Width + hasloPracownika[0].Location.X + 5, 15);
            rola[0].Text = "rola pracownika";
            rola[0].BorderStyle = BorderStyle.FixedSingle;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(10, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imieE.Width + imieE.Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            loginPracownikaE = new RichTextBox();
            loginPracownikaE.Width = 150;
            loginPracownikaE.Height = 30;
            loginPracownikaE.Location = new Point(nazwiskoE.Width + nazwiskoE.Location.X + 5, 50);
            loginPracownikaE.Text = "";
            loginPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            hasloPracownikaE = new RichTextBox();
            hasloPracownikaE.Width = 150;
            hasloPracownikaE.Height = 30;
            hasloPracownikaE.Location = new Point(loginPracownikaE.Width + loginPracownikaE.Location.X + 5, 50);
            hasloPracownikaE.Text = "";
            hasloPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            rolaPracownika = new ComboBox();
            rolaPracownika.Width = 150;
            rolaPracownika.Height = 30;
            rolaPracownika.Location = new Point(hasloPracownikaE.Width + hasloPracownikaE.Location.X + 5, 50);
            rolaPracownika.Text = "Administrator";
            rolaPracownika.Enabled = false;

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, rolaPracownika.Location.Y + rolaPracownika.Height + 5);
            Akceptuj.Click += dodajPr;

            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(loginPracownika[0]);
            prawy.Controls.Add(hasloPracownika[0]);
            prawy.Controls.Add(rola[0]);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(loginPracownikaE);
            prawy.Controls.Add(hasloPracownikaE);
            prawy.Controls.Add(rolaPracownika);
            prawy.Controls.Add(Akceptuj);
        }

        private void zapisDoPliku()
        {
            if (File.Exists(@"D:\testzapisu\klienci.txt"))
                {
                    if (File.Exists(@"D:\testzapisu\klienciKopia.txt"))
                    {
                        File.Replace(@"D:\testzapisu\klienci.txt", @"D:\testzapisu\klienciKopia.txt", @"D:\testzapisu\klienciBackup.txt");
                        File.Delete(@"D:\testzapisu\klienci.txt");
                    }
                    else
                    {
                        File.Copy(@"D:\testzapisu\klienci.txt", @"D:\testzapisu\klienciKopia.txt");
                        File.Delete(@"D:\testzapisu\klienci.txt");
                    }
                }
                for (int i = 0; i < klient.Length; i++)
                {
                    if (klient[i] != null)
                    {
                        File.AppendAllText(@"D:\testzapisu\klienci.txt",
                        klient[i].id + ":" + klient[i].imie + ":" + klient[i].nazwisko + ":" +
                        klient[i].nrtelefonu + ":" + klient[i].aktywny + Environment.NewLine);
                    }
                    else
                    {
                        break;
                    }
                }
                if (File.Exists(@"D:\testzapisu\sprzety.txt"))
                {
                    if (File.Exists(@"D:\testzapisu\sprzetyKopia.txt"))
                    {
                        File.Replace(@"D:\testzapisu\sprzety.txt", @"D:\testzapisu\sprzetyKopia.txt", @"D:\testzapisu\sprzetyBackup.txt");
                        File.Delete(@"D:\testzapisu\sprzety.txt");
                    }
                    else
                    {
                        File.Copy(@"D:\testzapisu\sprzety.txt", @"D:\testzapisu\sprzetyKopia.txt");
                        File.Delete(@"D:\testzapisu\sprzety.txt");
                    }
                }
                for (int i = 0; i < sprzet.Length; i++)
                {
                    if (sprzet[i] != null)
                    {
                        File.AppendAllText(@"D:\testzapisu\sprzety.txt",
                        sprzet[i].idSprzetu + ":" + sprzet[i].typ + ":" + sprzet[i].producent + ":" +
                        sprzet[i].opis + ":" + sprzet[i].idWlasciciela + ":" +
                        sprzet[i].status + ":" + sprzet[i].dataPrzyjecia + ":" + sprzet[i].dataOdbioru + Environment.NewLine);
                    }
                    else
                    {
                        break;
                    }
                }
                if (File.Exists(@"D:\testzapisu\pracownicy.txt"))
                {
                    if (File.Exists(@"D:\testzapisu\pracownicyKopia.txt"))
                    {
                        File.Replace(@"D:\testzapisu\pracownicy.txt", @"D:\testzapisu\pracownicyKopia.txt", @"D:\testzapisu\pracownicyBackup.txt");
                        File.Delete(@"D:\testzapisu\pracownicy.txt");
                    }
                    else
                    {
                        File.Copy(@"D:\testzapisu\pracownicy.txt", @"D:\testzapisu\pracownicyKopia.txt");
                        File.Delete(@"D:\testzapisu\pracownicy.txt");
                    }
                }
                for (int i = 0; i < pracownik.Length; i++)
                {
                    if (pracownik[i] != null)
                    {
                        File.AppendAllText(@"D:\testzapisu\pracownicy.txt",
                        pracownik[i].id + ":" + pracownik[i].imie + ":" + pracownik[i].nazwisko + ":" +
                        pracownik[i].login + ":" + pracownik[i].haslo + ":" +
                        pracownik[i].rola + ":" + pracownik[i].aktywny + Environment.NewLine);
                    }
                    else
                    {
                        break;
                    }
                }
                if (File.Exists(@"D:\testzapisu\serwisy.txt"))
                {
                    if (File.Exists(@"D:\testzapisu\serwisyKopia.txt"))
                    {
                        File.Replace(@"D:\testzapisu\serwisy.txt", @"D:\testzapisu\serwisyKopia.txt", @"D:\testzapisu\serwisyBackup.txt");
                    }
                    else
                    {
                        File.Copy(@"D:\testzapisu\serwisy.txt", @"D:\testzapisu\serwisyKopia.txt");
                        File.Delete(@"D:\testzapisu\serwisy.txt");
                    }
                }
                for (int i = 0; i < serwis.Length; i++)
                {
                    if (serwis[i] != null)
                    {
                        File.AppendAllText(@"D:\testzapisu\serwisy.txt",
                        serwis[i].idSprzetu + ":" + serwis[i].nazwiskoSerwisanta + ":" + serwis[i].opis + ":" +
                        serwis[i].koszt + Environment.NewLine);
                    }
                    else
                    {
                        break;
                    }
                }
        }

        private void budujPanelLogowania()
        {
            lewy.Controls.Clear();
            prawy.Controls.Clear();
            loginLabel = new Label();
            hasloLabel = new Label();
            login = new TextBox();
            haslo = new TextBox();
            zaloguj = new Button();
            loginLabel.Location = new Point(10, 20);
            loginLabel.AutoSize = true;
            loginLabel.Text = "Login:";
            login.Location = new Point(10, loginLabel.Location.Y + loginLabel.Height + 5);
            login.Width = lewy.Width - 20;
            hasloLabel.Location = new Point(10, login.Location.Y + login.Height + 5);
            hasloLabel.AutoSize = true;
            hasloLabel.Text = "Hasło:";
            haslo.Location = new Point(10, hasloLabel.Location.Y + hasloLabel.Height + 5);
            haslo.Width = lewy.Width - 20;
            zaloguj.Location = new Point(10, haslo.Location.Y + haslo.Height + 5);
            zaloguj.Text = "Zaloguj";
            zaloguj.Click += zaloguj_Click;
            zamknij = new Button();
            zamknij.Text = "Zamknij";
            zamknij.Location = new Point(10, zaloguj.Location.Y + zaloguj.Height + 5);
            zamknij.Click += wylacz;
            lewy.Text = "Logowanie";
            lewy.Controls.Add(loginLabel);
            lewy.Controls.Add(login);
            lewy.Controls.Add(hasloLabel);
            lewy.Controls.Add(haslo);
            lewy.Controls.Add(zaloguj);
            lewy.Controls.Add(zamknij);
        }

        private void wylacz(object sender, EventArgs e)
        {
            this.Close();
        }

        private void budujPanelSerwisanta()
        {
            lewy.Controls.Clear();
            prawy.Controls.Clear();

            wyswietlSerwisy = new Button();
            wyswietlSerwisy.Width = lewy.Width - 20;
            wyswietlSerwisy.Text = "Wyświetl serwisy";
            wyswietlSerwisy.Location = new Point(10, 20);
            wyswietlSerwisy.Click += wyswietlanieSerwisow;

            zarzadzajSerwisami = new Button();
            zarzadzajSerwisami.Width = lewy.Width - 20;
            zarzadzajSerwisami.Text = "Zarządzaj serwisami";
            zarzadzajSerwisami.Location = new Point(10, wyswietlSerwisy.Location.Y+wyswietlSerwisy.Height+5);
            zarzadzajSerwisami.Click += zarzadzanieSerwisami;

            wyloguj = new Button();
            wyloguj.Width = lewy.Width - 20;
            wyloguj.Text = "Wyloguj";
            wyloguj.Location = new Point(10, zarzadzajSerwisami.Location.Y + zarzadzajSerwisami.Height + 5);
            wyloguj.Click += wylogowanie;

            lewy.Controls.Add(wyswietlSerwisy);
            lewy.Controls.Add(zarzadzajSerwisami);
            lewy.Controls.Add(wyloguj);
        }

        private void budujPanelObslugiKlienta()
        {
            lewy.Controls.Clear();
            prawy.Controls.Clear();

            wyswietlKlientow = new Button();
            wyswietlKlientow.Width = lewy.Width - 20;
            wyswietlKlientow.Text = "Wyświetl Klientów";
            wyswietlKlientow.Location = new Point(10, 20);
            wyswietlKlientow.Click += wyswietlanieKlientow;

            wyswietlSprzet = new Button();
            wyswietlSprzet.Width = lewy.Width - 20;
            wyswietlSprzet.Text = "Wyświetl Sprzęt";
            wyswietlSprzet.Location = new Point(10, wyswietlKlientow.Location.Y + wyswietlKlientow.Height + 5);
            wyswietlSprzet.Click += wyswietlanieSprzetow;

            zarzadzajklientami = new Button();
            zarzadzajklientami.Width = lewy.Width - 20;
            zarzadzajklientami.Text = "Zarządzaj Klientami";
            zarzadzajklientami.Location = new Point(10, wyswietlSprzet.Location.Y + wyswietlSprzet.Height + 5);
            zarzadzajklientami.Click += zarzadzanieKlientami;

            zarzadzajSprzetem = new Button();
            zarzadzajSprzetem.Width = lewy.Width - 20;
            zarzadzajSprzetem.Text = "Zarządzaj Sprzętem";
            zarzadzajSprzetem.Location = new Point(10, zarzadzajklientami.Location.Y + zarzadzajklientami.Height + 5);
            zarzadzajSprzetem.Click += zarzadzanieSprzetem;

            wyswietlSerwisy = new Button();
            wyswietlSerwisy.Width = lewy.Width - 20;
            wyswietlSerwisy.Text = "Wyświetl serwisy";
            wyswietlSerwisy.Location = new Point(10, zarzadzajSprzetem.Location.Y + zarzadzajSprzetem.Height + 5);
            wyswietlSerwisy.Click += wyswietlanieSerwisow;

            zarzadzajSerwisami = new Button();
            zarzadzajSerwisami.Width = lewy.Width - 20;
            zarzadzajSerwisami.Text = "Zarządzaj serwisami";
            zarzadzajSerwisami.Location = new Point(10, wyswietlSerwisy.Location.Y + wyswietlSerwisy.Height + 5);
            zarzadzajSerwisami.Click += zarzadzanieSerwisami;

            wyloguj = new Button();
            wyloguj.Width = lewy.Width - 20;
            wyloguj.Text = "Wyloguj";
            wyloguj.Location = new Point(10, zarzadzajSerwisami.Location.Y + zarzadzajSerwisami.Height + 5);
            wyloguj.Click += wylogowanie;

            lewy.Controls.Add(wyswietlKlientow);
            lewy.Controls.Add(wyswietlSprzet);
            lewy.Controls.Add(zarzadzajklientami);
            lewy.Controls.Add(zarzadzajSprzetem);
            lewy.Controls.Add(wyswietlSerwisy);
            lewy.Controls.Add(zarzadzajSerwisami);
            lewy.Controls.Add(wyloguj);
        }

        private void budujPanelAdministratora()
        {
            lewy.Controls.Clear();
            prawy.Controls.Clear();

            wyswietlKlientow = new Button();
            wyswietlKlientow.Width = lewy.Width - 20;
            wyswietlKlientow.Text = "Wyświetl Klientów";
            wyswietlKlientow.Location = new Point(10, 20);
            wyswietlKlientow.Click += wyswietlanieKlientow;

            wyswietlSprzet = new Button();
            wyswietlSprzet.Width = lewy.Width - 20;
            wyswietlSprzet.Text = "Wyświetl Sprzęt";
            wyswietlSprzet.Location = new Point(10, wyswietlKlientow.Location.Y + wyswietlKlientow.Height + 5);
            wyswietlSprzet.Click += wyswietlanieSprzetow;

            zarzadzajklientami = new Button();
            zarzadzajklientami.Width = lewy.Width - 20;
            zarzadzajklientami.Text = "Zarządzaj Klientami";
            zarzadzajklientami.Location = new Point(10, wyswietlSprzet.Location.Y + wyswietlSprzet.Height + 5);
            zarzadzajklientami.Click += zarzadzanieKlientami;

            zarzadzajSprzetem = new Button();
            zarzadzajSprzetem.Width = lewy.Width - 20;
            zarzadzajSprzetem.Text = "Zarządzaj Sprzętem";
            zarzadzajSprzetem.Location = new Point(10, zarzadzajklientami.Location.Y + zarzadzajklientami.Height + 5);
            zarzadzajSprzetem.Click += zarzadzanieSprzetem;

            wyswietlSerwisy = new Button();
            wyswietlSerwisy.Width = lewy.Width - 20;
            wyswietlSerwisy.Text = "Wyświetl serwisy";
            wyswietlSerwisy.Location = new Point(10, zarzadzajSprzetem.Location.Y + zarzadzajSprzetem.Height + 5);
            wyswietlSerwisy.Click += wyswietlanieSerwisow;

            zarzadzajSerwisami = new Button();
            zarzadzajSerwisami.Width = lewy.Width - 20;
            zarzadzajSerwisami.Text = "Zarządzaj serwisami";
            zarzadzajSerwisami.Location = new Point(10, wyswietlSerwisy.Location.Y + wyswietlSerwisy.Height + 5);
            zarzadzajSerwisami.Click += zarzadzanieSerwisami;

            wyswietlPracownikow = new Button();
            wyswietlPracownikow.Width = lewy.Width - 20;
            wyswietlPracownikow.Text = "Wyświetl Pracowników";
            wyswietlPracownikow.Location = new Point(10, zarzadzajSerwisami.Location.Y + zarzadzajSerwisami.Height + 5);
            wyswietlPracownikow.Click += wyswietlaniePracowników;

            zarzadzajPracownikami = new Button();
            zarzadzajPracownikami.Width = lewy.Width - 20;
            zarzadzajPracownikami.Text = "Zarządzaj Pracownikami";
            zarzadzajPracownikami.Location = new Point(10, wyswietlPracownikow.Location.Y + wyswietlPracownikow.Height + 5);
            zarzadzajPracownikami.Click += zarzadzaniePracownikami;

            wyloguj = new Button();
            wyloguj.Width = lewy.Width - 20;
            wyloguj.Text = "Wyloguj";
            wyloguj.Location = new Point(10, zarzadzajPracownikami.Location.Y + zarzadzajPracownikami.Height + 5);
            wyloguj.Click += wylogowanie;

            lewy.Controls.Add(wyswietlKlientow);
            lewy.Controls.Add(wyswietlSprzet);
            lewy.Controls.Add(zarzadzajklientami);
            lewy.Controls.Add(zarzadzajSprzetem);
            lewy.Controls.Add(wyswietlSerwisy);
            lewy.Controls.Add(zarzadzajSerwisami);
            lewy.Controls.Add(wyswietlPracownikow);
            lewy.Controls.Add(zarzadzajPracownikami);
            lewy.Controls.Add(wyloguj);
        }

        private void zaloguj_Click(object sender, EventArgs e)
        {
            wczytajDane();
            zalogowano = false;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i]!=null)
                {
                    if (login.Text == pracownik[i].login && haslo.Text == pracownik[i].haslo)
                    {
                        zalogowano = true;
                        if (pracownik[i].rola == "Administrator")
                        {
                            budujPanelAdministratora();
                            zalogowany = pracownik[i];
                            lewy.Text = "Panel Administratora - " + pracownik[i].nazwisko;
                            wyswietlNoweSerwisy();
                        }
                        else if (pracownik[i].rola == "Serwisant")
                        {
                            budujPanelSerwisanta();
                            zalogowany = pracownik[i];
                            lewy.Text = "Panel Serwisanta - " + pracownik[i].nazwisko;
                            wyswietlNoweSerwisy();
                        }
                        else if (pracownik[i].rola == "Biuro")
                        {
                            budujPanelObslugiKlienta();
                            zalogowany = pracownik[i];
                            lewy.Text = "Panel Obsługi Klienta - " + pracownik[i].nazwisko;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            if (!zalogowano)
            {
                MessageBox.Show("Błędny login lub hasło.");
            }
        }

        private void wyswietlNoweSerwisy()
        {
            prawy.Controls.Clear();
            id = new Label[serwis.Length + 1];
            idSprzetu = new Label[serwis.Length + 1];
            status = new Label[serwis.Length + 1];
            koszt = new Label[serwis.Length + 1];
            opis = new RichTextBox[serwis.Length + 1];
            int j = serwis.Length - 1;
            int pozycjonowanie = 1;
            for (int i = 0; i <= serwis.Length; i++)
            {
                if (i == 0)
                {
                    id[i] = new Label();
                    id[i].Width = 70;
                    id[i].Height = 30;
                    id[i].Location = new Point(10, 10 + id[i].Height * i + 5);
                    id[i].Text = "Serwisant";
                    id[i].BorderStyle = BorderStyle.FixedSingle;

                    idSprzetu[i] = new Label();
                    idSprzetu[i].Width = 70;
                    idSprzetu[i].Height = 30;
                    idSprzetu[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * i + 5);
                    idSprzetu[i].Text = "Id Sprzętu";
                    idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                    Label opisa = new Label();
                    opisa.Width = 150;
                    opisa.Height = 30;
                    opisa.Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * i + 5);
                    opisa.Text = "Opis problemu";
                    opisa.BorderStyle = BorderStyle.FixedSingle;

                    status[i] = new Label();
                    status[i].Width = 150;
                    status[i].Height = 30;
                    status[i].Location = new Point(opisa.Width + opisa.Location.X + 5, 10 + opisa.Height * i + 5);
                    status[i].Text = "Status";
                    status[i].BorderStyle = BorderStyle.FixedSingle;

                    koszt[i] = new Label();
                    koszt[i].Width = 150;
                    koszt[i].Height = 30;
                    koszt[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * i + 5);
                    koszt[i].Text = "Koszt";
                    koszt[i].BorderStyle = BorderStyle.FixedSingle;

                    prawy.Controls.Add(id[i]);
                    prawy.Controls.Add(idSprzetu[i]);
                    prawy.Controls.Add(opisa);
                    prawy.Controls.Add(status[i]);
                    prawy.Controls.Add(koszt[i]);
                }
                else
                {
                    if (serwis[j + 1] != null)
                    {
                        if (sprzet[j+1].status=="Do naprawy")
                        {
                            id[i] = new Label();
                            id[i].Width = 70;
                            id[i].Height = 30;
                            id[i].Location = new Point(10, 10 + id[i].Height * pozycjonowanie + 5);
                            id[i].Text = serwis[j + 1].nazwiskoSerwisanta;
                            id[i].BorderStyle = BorderStyle.FixedSingle;

                            idSprzetu[i] = new Label();
                            idSprzetu[i].Width = 70;
                            idSprzetu[i].Height = 30;
                            idSprzetu[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * pozycjonowanie + 5);
                            idSprzetu[i].Text = serwis[j + 1].idSprzetu + "";
                            idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                            opis[i] = new RichTextBox();
                            opis[i].Width = 150;
                            opis[i].Height = 30;
                            opis[i].Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * pozycjonowanie + 5);
                            for (int k = 0; k < sprzet.Length; k++)
                            {
                                if (serwis[j + 1] != null && sprzet[k] != null)
                                    if (serwis[j + 1].idSprzetu == sprzet[k].idSprzetu)
                                    {
                                        opis[i].Text = sprzet[k].opis;
                                    }
                            }


                            status[i] = new Label();
                            status[i].Width = 150;
                            status[i].Height = 30;
                            status[i].Location = new Point(opis[i].Width + opis[i].Location.X + 5, 10 + opis[i].Height * pozycjonowanie + 5);

                            for (int k = 0; k < sprzet.Length; k++)
                            {
                                if (serwis[j + 1] != null && sprzet[k] != null)
                                    if (serwis[j + 1].idSprzetu == sprzet[k].idSprzetu)
                                    {
                                        status[i].Text = sprzet[k].status;
                                    }
                            }
                            status[i].BorderStyle = BorderStyle.FixedSingle;

                            koszt[i] = new Label();
                            koszt[i].Width = 150;
                            koszt[i].Height = 30;
                            koszt[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * pozycjonowanie + 5);
                            koszt[i].Text = serwis[j + 1].koszt + "";
                            koszt[i].BorderStyle = BorderStyle.FixedSingle;

                            prawy.Controls.Add(id[i]);
                            prawy.Controls.Add(idSprzetu[i]);
                            prawy.Controls.Add(opis[i]);
                            prawy.Controls.Add(status[i]);
                            prawy.Controls.Add(koszt[i]);
                            pozycjonowanie++;
                        }
                    }
                }
                j--;
            }
        }

        private void wyswietlanieKlientow(object sender, EventArgs e)
        {
            budujBazeklientow();
        }

        private void budujBazeklientow()
        {
            prawy.Controls.Clear();
            sortKl.Controls.Clear();
            id = new Label[klient.Length + 1];
            imie = new Label[klient.Length + 1];
            nazwisko = new Label[klient.Length + 1];
            telefon = new Label[klient.Length + 1];
            int przelicznik = 1;
            for (int i = 0; i <= klient.Length; i++)
            {
                if (i == 0)
                {
                    id[i] = new Label();
                    id[i].Width = 25;
                    id[i].Height = 30;
                    id[i].Location = new Point(10, 10 + id[i].Height * i + 5);
                    id[i].Text = "Id";
                    id[i].BorderStyle = BorderStyle.FixedSingle;

                    imie[i] = new Label();
                    imie[i].Width = 150;
                    imie[i].Height = 30;
                    imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * i + 5);
                    imie[i].Text = "Imie";
                    imie[i].BorderStyle = BorderStyle.FixedSingle;

                    nazwisko[i] = new Label();
                    nazwisko[i].Width = 150;
                    nazwisko[i].Height = 30;
                    nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * i + 5);
                    nazwisko[i].Text = "Nazwisko";
                    nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                    telefon[i] = new Label();
                    telefon[i].Width = 150;
                    telefon[i].Height = 30;
                    telefon[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * i + 5);
                    telefon[i].Text = "Numer telefonu";
                    telefon[i].BorderStyle = BorderStyle.FixedSingle;

                    sortowaniePoNazwisku = new Label();
                    sortowaniePoNazwisku.Height = 30;
                    sortowaniePoNazwisku.Width = 150;
                    sortowaniePoNazwisku.Text = "Szukaj po nazwisku";
                    sortowaniePoNazwisku.Location = new Point(telefon[i].Width + telefon[i].Location.X + 15, 15);
                    sortowaniePoNazwisku.BorderStyle = BorderStyle.FixedSingle;

                    sortPoNazwisku = new RichTextBox();
                    sortPoNazwisku.Width = 150;
                    sortPoNazwisku.Height = 30;
                    sortPoNazwisku.Location = new Point(telefon[i].Width + telefon[i].Location.X + 15, 50);
                    sortPoNazwisku.Text = "";
                    sortPoNazwisku.TextChanged += szukajKlienta;

                    sortowaniePoTelefonie = new Label();
                    sortowaniePoTelefonie.Height = 30;
                    sortowaniePoTelefonie.Width = 150;
                    sortowaniePoTelefonie.Text = "Szukaj po numerze telefonu";
                    sortowaniePoTelefonie.Location = new Point(sortPoNazwisku.Width + sortPoNazwisku.Location.X + 5, 15);
                    sortowaniePoTelefonie.BorderStyle = BorderStyle.FixedSingle;

                    sortPoTelefonie = new RichTextBox();
                    sortPoTelefonie.Width = 150;
                    sortPoTelefonie.Height = 30;
                    sortPoTelefonie.Location = new Point(sortPoNazwisku.Width + sortPoNazwisku.Location.X + 5, 50);
                    sortPoTelefonie.Text = "";
                    sortPoTelefonie.TextChanged += szukajKlientaPoTel;

                    sortKl.Location= new Point(0, 5);

                    prawy.Controls.Add(id[i]);
                    prawy.Controls.Add(imie[i]);
                    prawy.Controls.Add(nazwisko[i]);
                    prawy.Controls.Add(telefon[i]);
                    prawy.Controls.Add(sortowaniePoNazwisku);
                    prawy.Controls.Add(sortPoNazwisku);
                    prawy.Controls.Add(sortowaniePoTelefonie);
                    prawy.Controls.Add(sortPoTelefonie);
                }
                else if (klient[i - 1]!=null)
                {
                    if (klient[i - 1].aktywny)
                    {

                        id[i] = new Label();
                        id[i].Width = 25;
                        id[i].Height = 30;
                        id[i].Location = new Point(10, 10 + id[i].Height * przelicznik + 5);
                        id[i].Text = klient[i - 1].id + "";
                        id[i].BorderStyle = BorderStyle.FixedSingle;

                        imie[i] = new Label();
                        imie[i].Width = 150;
                        imie[i].Height = 30;
                        imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * przelicznik + 5);
                        imie[i].Text = klient[i - 1].imie;
                        imie[i].BorderStyle = BorderStyle.FixedSingle;

                        nazwisko[i] = new Label();
                        nazwisko[i].Width = 150;
                        nazwisko[i].Height = 30;
                        nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * przelicznik + 5);
                        nazwisko[i].Text = klient[i - 1].nazwisko;
                        nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                        telefon[i] = new Label();
                        telefon[i].Width = 150;
                        telefon[i].Height = 30;
                        telefon[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * przelicznik + 5);
                        telefon[i].Text = klient[i - 1].nrtelefonu;
                        telefon[i].BorderStyle = BorderStyle.FixedSingle;

                        sortKl.Width = id[i].Location.X + telefon[i].Location.X + telefon[i].Width;
                        sortKl.Height = 10 + nazwisko[i].Height * (przelicznik + 1) + 20;
                        sortKl.Controls.Add(id[i]);
                        sortKl.Controls.Add(imie[i]);
                        sortKl.Controls.Add(nazwisko[i]);
                        sortKl.Controls.Add(telefon[i]);
                        przelicznik++;
                    }
                }
            }
            prawy.Controls.Add(sortKl);
        }

        private void szukajKlientaPoTel(object sender, EventArgs e)
        {
            sortKl.Controls.Clear();
            int przelicznik = 1;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i] != null)
                {
                    if (klient[i].nrtelefonu.Contains(sortPoTelefonie.Text) || sortPoTelefonie.Text == "")
                    {
                        id[i] = new Label();
                        id[i].Width = 25;
                        id[i].Height = 30;
                        id[i].Location = new Point(10, 10 + id[i].Height * przelicznik + 5);
                        id[i].Text = klient[i].id + "";
                        id[i].BorderStyle = BorderStyle.FixedSingle;

                        imie[i] = new Label();
                        imie[i].Width = 150;
                        imie[i].Height = 30;
                        imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * przelicznik + 5);
                        imie[i].Text = klient[i].imie;
                        imie[i].BorderStyle = BorderStyle.FixedSingle;

                        nazwisko[i] = new Label();
                        nazwisko[i].Width = 150;
                        nazwisko[i].Height = 30;
                        nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * przelicznik + 5);
                        nazwisko[i].Text = klient[i].nazwisko;
                        nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                        telefon[i] = new Label();
                        telefon[i].Width = 150;
                        telefon[i].Height = 30;
                        telefon[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * przelicznik + 5);
                        telefon[i].Text = klient[i].nrtelefonu;
                        telefon[i].BorderStyle = BorderStyle.FixedSingle;

                        sortKl.Controls.Add(id[i]);
                        sortKl.Controls.Add(imie[i]);
                        sortKl.Controls.Add(nazwisko[i]);
                        sortKl.Controls.Add(telefon[i]);
                        przelicznik++;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void szukajKlienta(object sender, EventArgs e)
        {
            sortKl.Controls.Clear();
            int przelicznik = 1;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i]!=null)
                {
                    if (klient[i].nazwisko.Contains(sortPoNazwisku.Text) || sortPoNazwisku.Text == "") 
                    {
                        id[i] = new Label();
                        id[i].Width = 25;
                        id[i].Height = 30;
                        id[i].Location = new Point(10, 10 + id[i].Height * przelicznik + 5);
                        id[i].Text = klient[i].id + "";
                        id[i].BorderStyle = BorderStyle.FixedSingle;

                        imie[i] = new Label();
                        imie[i].Width = 150;
                        imie[i].Height = 30;
                        imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * przelicznik + 5);
                        imie[i].Text = klient[i].imie;
                        imie[i].BorderStyle = BorderStyle.FixedSingle;

                        nazwisko[i] = new Label();
                        nazwisko[i].Width = 150;
                        nazwisko[i].Height = 30;
                        nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * przelicznik + 5);
                        nazwisko[i].Text = klient[i].nazwisko;
                        nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                        telefon[i] = new Label();
                        telefon[i].Width = 150;
                        telefon[i].Height = 30;
                        telefon[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * przelicznik + 5);
                        telefon[i].Text = klient[i].nrtelefonu;
                        telefon[i].BorderStyle = BorderStyle.FixedSingle;

                        sortKl.Controls.Add(id[i]);
                        sortKl.Controls.Add(imie[i]);
                        sortKl.Controls.Add(nazwisko[i]);
                        sortKl.Controls.Add(telefon[i]);
                        przelicznik++;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private void wyswietlanieSprzetow(object sender, EventArgs e)
        {
            budujBazeSprzetow();
        }

        private void budujBazeSprzetow()
        {
            prawy.Controls.Clear();
            idSprzetu = new Label[sprzet.Length + 1];
            typ = new Label[sprzet.Length + 1];
            producent = new Label[sprzet.Length + 1];
            opis = new RichTextBox[sprzet.Length + 1];
            idWlasciciela = new Label[sprzet.Length + 1];
            status = new Label[sprzet.Length + 1];
            dataOdbioru = new Label[sprzet.Length + 1];
            dataPrzyjecia = new Label[sprzet.Length + 1];
            for (int i = 0; i <= sprzet.Length; i++)
            {
                if (i == 0)
                {
                    idSprzetu[i] = new Label();
                    idSprzetu[i].Width = 25;
                    idSprzetu[i].Height = 30;
                    idSprzetu[i].Location = new Point(10, 10 + idSprzetu[i].Height * i + 5);
                    idSprzetu[i].Text = "Id";
                    idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                    typ[i] = new Label();
                    typ[i].Width = 150;
                    typ[i].Height = 30;
                    typ[i].Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * i + 5);
                    typ[i].Text = "Typ sprzętu";
                    typ[i].BorderStyle = BorderStyle.FixedSingle;

                    producent[i] = new Label();
                    producent[i].Width = 150;
                    producent[i].Height = 30;
                    producent[i].Location = new Point(typ[i].Width + typ[i].Location.X + 5, 10 + typ[i].Height * i + 5);
                    producent[i].Text = "Nazwa producent";
                    producent[i].BorderStyle = BorderStyle.FixedSingle;

                    Label opisa = new Label();
                    opisa.Width = 150;
                    opisa.Height = 30;
                    opisa.Location = new Point(producent[i].Location.X + producent[i].Width + 5, 10 + producent[i].Height * i + 5);
                    opisa.Text = "Opis problemu";
                    opisa.BorderStyle = BorderStyle.FixedSingle;

                    idWlasciciela[i] = new Label();
                    idWlasciciela[i].Width = 70;
                    idWlasciciela[i].Height = 30;
                    idWlasciciela[i].Location = new Point(opisa.Width + opisa.Location.X + 5, 10 + opisa.Height * i + 5);
                    idWlasciciela[i].Text = "Id właściciela";
                    idWlasciciela[i].BorderStyle = BorderStyle.FixedSingle;

                    status[i] = new Label();
                    status[i].Width = 150;
                    status[i].Height = 30;
                    status[i].Location = new Point(idWlasciciela[i].Width + idWlasciciela[i].Location.X + 5, 10 + idWlasciciela[i].Height * i + 5);
                    status[i].Text = "Status naprawy";
                    status[i].BorderStyle = BorderStyle.FixedSingle;

                    dataPrzyjecia[i] = new Label();
                    dataPrzyjecia[i].Width = 150;
                    dataPrzyjecia[i].Height = 30;
                    dataPrzyjecia[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * i + 5);
                    dataPrzyjecia[i].Text = "Data przyjęcia";
                    dataPrzyjecia[i].BorderStyle = BorderStyle.FixedSingle;

                    dataOdbioru[i] = new Label();
                    dataOdbioru[i].Width = 150;
                    dataOdbioru[i].Height = 30;
                    dataOdbioru[i].Location = new Point(dataPrzyjecia[i].Width + dataPrzyjecia[i].Location.X + 5, 10 + dataPrzyjecia[i].Height * i + 5);
                    dataOdbioru[i].Text = "Data odbioru";
                    dataOdbioru[i].BorderStyle = BorderStyle.FixedSingle;


                    prawy.Controls.Add(idSprzetu[i]);
                    prawy.Controls.Add(typ[i]);
                    prawy.Controls.Add(producent[i]);
                    prawy.Controls.Add(opisa);
                    prawy.Controls.Add(idWlasciciela[i]);
                    prawy.Controls.Add(status[i]);
                    prawy.Controls.Add(dataOdbioru[i]);
                    prawy.Controls.Add(dataPrzyjecia[i]);
                }
                else if (sprzet[i - 1] != null)
                {
                    idSprzetu[i] = new Label();
                    idSprzetu[i].Width = 25;
                    idSprzetu[i].Height = 30;
                    idSprzetu[i].Location = new Point(10, 10 + idSprzetu[i].Height * i + 5);
                    idSprzetu[i].Text = sprzet[i - 1].idSprzetu + "";
                    idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                    typ[i] = new Label();
                    typ[i].Width = 150;
                    typ[i].Height = 30;
                    typ[i].Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * i + 5);
                    typ[i].Text = sprzet[i - 1].typ;
                    typ[i].BorderStyle = BorderStyle.FixedSingle;

                    producent[i] = new Label();
                    producent[i].Width = 150;
                    producent[i].Height = 30;
                    producent[i].Location = new Point(typ[i].Width + typ[i].Location.X + 5, 10 + typ[i].Height * i + 5);
                    producent[i].Text = sprzet[i - 1].producent;
                    producent[i].BorderStyle = BorderStyle.FixedSingle;

                    opis[i] = new RichTextBox();
                    opis[i].Width = 150;
                    opis[i].Height = 30;
                    opis[i].Location = new Point(producent[i].Location.X + producent[i].Width + 5, 10 + producent[i].Height * i + 5);
                    opis[i].Text = sprzet[i - 1].opis;
                    opis[i].Enabled = false;

                    idWlasciciela[i] = new Label();
                    idWlasciciela[i].Width = 70;
                    idWlasciciela[i].Height = 30;
                    idWlasciciela[i].Location = new Point(opis[i].Width + opis[i].Location.X + 5, 10 + opis[i].Height * i + 5);
                    idWlasciciela[i].Text = sprzet[i - 1].idWlasciciela + "";
                    idWlasciciela[i].BorderStyle = BorderStyle.FixedSingle;

                    status[i] = new Label();
                    status[i].Width = 150;
                    status[i].Height = 30;
                    status[i].Location = new Point(idWlasciciela[i].Width + idWlasciciela[i].Location.X + 5, 10 + idWlasciciela[i].Height * i + 5);
                    status[i].Text = sprzet[i - 1].status;
                    status[i].BorderStyle = BorderStyle.FixedSingle;

                    dataPrzyjecia[i] = new Label();
                    dataPrzyjecia[i].Width = 150;
                    dataPrzyjecia[i].Height = 30;
                    dataPrzyjecia[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * i + 5);
                    dataPrzyjecia[i].Text = sprzet[i-1].dataPrzyjecia+"";
                    dataPrzyjecia[i].BorderStyle = BorderStyle.FixedSingle;

                    dataOdbioru[i] = new Label();
                    dataOdbioru[i].Width = 150;
                    dataOdbioru[i].Height = 30;
                    dataOdbioru[i].Location = new Point(dataPrzyjecia[i].Width + dataPrzyjecia[i].Location.X + 5, 10 + dataPrzyjecia[i].Height * i + 5);
                    dataOdbioru[i].Text = sprzet[i - 1].dataOdbioru + "";
                    dataOdbioru[i].BorderStyle = BorderStyle.FixedSingle;

                    prawy.Controls.Add(idSprzetu[i]);
                    prawy.Controls.Add(typ[i]);
                    prawy.Controls.Add(producent[i]);
                    prawy.Controls.Add(opis[i]);
                    prawy.Controls.Add(idWlasciciela[i]);
                    prawy.Controls.Add(status[i]);
                    prawy.Controls.Add(dataOdbioru[i]);
                    prawy.Controls.Add(dataPrzyjecia[i]);
                }
            }
        }

        private void zarzadzanieKlientami(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            Dodaj = new Button();
            Dodaj.Location = new Point(10, 10);
            Dodaj.Text = "Dodaj nowego klienta";
            Dodaj.Click += dodajKlienta;

            Zmodyfikuj = new Button();
            Zmodyfikuj.Text = "Zmodyfikuj dane klienta";
            Zmodyfikuj.Location = new Point(10, Dodaj.Location.Y + Dodaj.Height + 5);
            Zmodyfikuj.Click += zmodyfikujKlienta;

            Usun = new Button();
            Usun.Text = "Usuń dane klienta";
            Usun.Location = new Point(10, Zmodyfikuj.Location.Y + Dodaj.Height + 5);
            Usun.Click += usunKlienta;

            prawy.Controls.Add(Dodaj);
            prawy.Controls.Add(Zmodyfikuj);
            prawy.Controls.Add(Usun);
        }

        private void usunKlienta(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            id = new Label[1];
            id[0] = new Label();
            id[0].Width = 70;
            id[0].Height = 30;
            id[0].Location = new Point(10, 15);
            id[0].Text = "Id";
            id[0].BorderStyle = BorderStyle.FixedSingle;

            imie = new Label[1];
            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(id[0].Width + id[0].Location.X + 5, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko = new Label[1];
            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            telefon = new Label[1];
            telefon[0] = new Label();
            telefon[0].Width = 150;
            telefon[0].Height = 30;
            telefon[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            telefon[0].Text = "Numer telefonu";
            telefon[0].BorderStyle = BorderStyle.FixedSingle;

            idE = new NumericUpDown();
            idE.Width = id[0].Width;
            idE.Height = 30;
            idE.Location = new Point(10, 50);
            idE.Value = 0;
            int maximum = -1;
            for (int i = 0; i < klient.Length+2; i++)
            {
                if (klient[i]!=null)
                {
                    maximum++;
                    Console.WriteLine(maximum);
                }
                else
                {
                    break;
                }
            }
            idE.Maximum = maximum;
            idE.BorderStyle = BorderStyle.FixedSingle;
            idE.Click += wyswietlEdytowanegoKlienta;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(id[0].Width + id[0].Location.X + 5, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imie[0].Width + imie[0].Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            telefonE = new RichTextBox();
            telefonE.Width = 150;
            telefonE.Height = 30;
            telefonE.Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 50);
            telefonE.Text = "";
            telefonE.BorderStyle = BorderStyle.FixedSingle;

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, telefonE.Location.Y + telefonE.Height + 5);
            Akceptuj.Click += usunKl;

            prawy.Controls.Add(id[0]);
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(telefon[0]);
            prawy.Controls.Add(idE);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(telefonE);
            prawy.Controls.Add(Akceptuj);
            Usun.Click -= usunKlienta;
        }

        private void usunKl(object sender, EventArgs e)
        {
            bool flaga = false;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i] != null)
                    if (Convert.ToInt32(idE.Text) == klient[i].id)
                    {
                        flaga = true;
                    }
            }
            if (flaga)
            {
                DialogResult result;
                result = MessageBox.Show("Czy napewno Usunąć?", "", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    for (int i = 0; i < klient.Length; i++)
                    {
                        if (klient[i] != null)
                            if (klient[i].id == Convert.ToInt32(idE.Text))
                            {
                                klient[i].imie = "Dane usunięte";
                                klient[i].nazwisko = "Dane usunięte";
                                klient[i].nrtelefonu = "Dane usunięte";
                                klient[i].aktywny = false;
                            }
                    }
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Poprawnie zmodyfikowano dane Klienta";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                    zapisDoPliku();
                }
                else
                {
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Nie zmodyfikowano danych Klienta";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                }
            }
            else
            {
                MessageBox.Show("Prosze podać poprawny numer id klienta.");
            }
            Akceptuj.Click -= usunKl;
        }

        private void zmodyfikujKlienta(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            id = new Label[1];
            id[0] = new Label();
            id[0].Width = 70;
            id[0].Height = 30;
            id[0].Location = new Point(10, 15);
            id[0].Text = "Id";
            id[0].BorderStyle = BorderStyle.FixedSingle;

            imie = new Label[1];
            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(id[0].Width + id[0].Location.X + 5, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko = new Label[1];
            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            telefon = new Label[1];
            telefon[0] = new Label();
            telefon[0].Width = 150;
            telefon[0].Height = 30;
            telefon[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            telefon[0].Text = "Numer telefonu";
            telefon[0].BorderStyle = BorderStyle.FixedSingle;

            idE = new NumericUpDown();
            idE.Width = id[0].Width;
            idE.Height = 30;
            idE.Location = new Point(10, 50);
            idE.Value = 0;
            int maximum = -1;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i] != null)
                {
                    maximum++;
                }
                else
                {
                    break;
                }
            }
            idE.Maximum = maximum;
            idE.BorderStyle = BorderStyle.FixedSingle;
            idE.ValueChanged += wyswietlEdytowanegoKlienta;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(id[0].Width + id[0].Location.X + 5, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imie[0].Width + imie[0].Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            telefonE = new RichTextBox();
            telefonE.Width = 150;
            telefonE.Height = 30;
            telefonE.Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 50);
            telefonE.Text = "";
            telefonE.BorderStyle = BorderStyle.FixedSingle;

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, telefonE.Location.Y + telefonE.Height + 5);
            Akceptuj.Click += zmodyfikujKl;

            prawy.Controls.Add(id[0]);
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(telefon[0]);
            prawy.Controls.Add(idE);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(telefonE);
            prawy.Controls.Add(Akceptuj);

            Zmodyfikuj.Click -= zmodyfikujKlienta;
        }

        private void wyswietlEdytowanegoKlienta(object sender, EventArgs e)
        {
            if (klient[Convert.ToInt32(idE.Value)]!=null)
            {
                if (klient[Convert.ToInt32(idE.Value)].aktywny)
                {
                    imieE.Text = klient[Convert.ToInt32(idE.Value)].imie;
                    nazwiskoE.Text = klient[Convert.ToInt32(idE.Value)].nazwisko;
                    telefonE.Text = klient[Convert.ToInt32(idE.Value)].nrtelefonu;
                }
                else
                {
                    for (int i = Convert.ToInt32(idE.Value) + 1; i < klient.Length; i++)
                    {
                        if (klient[Convert.ToInt32(idE.Value) + 1] != null)
                        {
                            if (klient[Convert.ToInt32(idE.Value) + 1].aktywny)
                            {
                                idE.Value += 1;
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
            }
            else
            {
                idE.Value = 0;
                MessageBox.Show("Niepoprawny identyfikator klienta.");
            }
        }

        private void dodajKlienta(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            imie = new Label[1];
            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(10, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko = new Label[1];
            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            telefon = new Label[1];
            telefon[0] = new Label();
            telefon[0].Width = 150;
            telefon[0].Height = 30;
            telefon[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            telefon[0].Text = "Numer telefonu";
            telefon[0].BorderStyle = BorderStyle.FixedSingle;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(10, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imie[0].Width + imie[0].Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            telefonE = new RichTextBox();
            telefonE.Width = 150;
            telefonE.Height = 30;
            telefonE.Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 50);
            telefonE.Text = "";
            telefonE.BorderStyle = BorderStyle.FixedSingle;

            idNowego = 0;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i]!=null)
                {
                    idNowego++;
                }
            }

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, telefonE.Location.Y + telefonE.Height + 5);
            Akceptuj.Click += dodajKl;
            
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(telefon[0]);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(telefonE);
            prawy.Controls.Add(Akceptuj);
            Dodaj.Click -= dodajKlienta;
        }

        private void dodajKl(object sender, EventArgs e)
        {
            klient[idNowego] = new klienci(idNowego, imieE.Text, nazwiskoE.Text, telefonE.Text, true);

                imie = new Label[1];
                imie[0] = new Label();
                imie[0].Width = 150;
                imie[0].Height = 30;
                imie[0].Location = new Point(10, 15);
                imie[0].Text = "Poprawnie dodano dane Klienta";
                imie[0].BorderStyle = BorderStyle.FixedSingle;
                prawy.Controls.Clear();
                prawy.Controls.Add(imie[0]);
                zapisDoPliku();
            Akceptuj.Click -= dodajKl;
        }

        private void zmodyfikujKl(object sender, EventArgs e)
        {
            bool flaga = false;
            for (int i = 0; i < klient.Length; i++)
            {
                if (klient[i] != null)
                if (Convert.ToInt32(idE.Text) == klient[i].id)
                {
                    flaga = true;
                }
            }
            if (flaga)
            {
                DialogResult result;
                result = MessageBox.Show("Czy napewno zmodyfikować?", "", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    nowy.id = Convert.ToInt32(idE.Text);
                    nowy.imie = imieE.Text;
                    nowy.nazwisko = nazwiskoE.Text;
                    nowy.nrtelefonu = telefonE.Text;
                    for (int i = 0; i < klient.Length; i++)
                    {
                        if(klient[i] != null)
                        if (klient[i].id == Convert.ToInt32(idE.Text))
                        {
                            klient[i] = nowy;
                        }
                    }
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Poprawnie zmodyfikowano dane Klienta";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                    zapisDoPliku();
                }
                else
                {
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Nie zmodyfikowano danych Klienta";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                }
            }
            else
            {
                MessageBox.Show("Prosze podać poprawny numer id klienta.");
            }
            Akceptuj.Click -= zmodyfikujKl;
        }

        private void zarzadzanieSprzetem(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            Dodaj = new Button();
            Dodaj.Location = new Point(10, 10);
            Dodaj.Text = "Dodaj nowy sprzet";
            Dodaj.Click += dodajSprzet;

            Zmodyfikuj = new Button();
            Zmodyfikuj.Text = "Zmodyfikuj dane sprzętu";
            Zmodyfikuj.Location = new Point(10, Dodaj.Location.Y + Dodaj.Height + 5);
            Zmodyfikuj.Click += zmodyfikujSprzet;

            prawy.Controls.Add(Dodaj);
            prawy.Controls.Add(Zmodyfikuj);
        }

        private void zmodyfikujSprzet(object sender, EventArgs e)
        {
            prawy.Controls.Clear();
            int liczbaSprzetow = 0;
            for (int i = 0; i < sprzet.Length; i++)
            {
                if (sprzet[i]!=null)
                {
                    liczbaSprzetow++;
                }
            }

            if (liczbaSprzetow>0)
            {

                idSprzetu = new Label[1];
                idSprzetu[0] = new Label();
                idSprzetu[0].Width = 70;
                idSprzetu[0].Height = 30;
                idSprzetu[0].Location = new Point(10, 15);
                idSprzetu[0].Text = "Id";
                idSprzetu[0].BorderStyle = BorderStyle.FixedSingle;

                typ = new Label[1];
                typ[0] = new Label();
                typ[0].Width = 150;
                typ[0].Height = 30;
                typ[0].Location = new Point(idSprzetu[0].Location.X + idSprzetu[0].Width + 5, 15);
                typ[0].Text = "Typ sprzętu";
                typ[0].BorderStyle = BorderStyle.FixedSingle;

                producent = new Label[1];
                producent[0] = new Label();
                producent[0].Width = 150;
                producent[0].Height = 30;
                producent[0].Location = new Point(typ[0].Width + typ[0].Location.X + 5, 15);
                producent[0].Text = "Nazwa producent";
                producent[0].BorderStyle = BorderStyle.FixedSingle;

                Label opisa = new Label();
                opisa.Width = 150;
                opisa.Height = 30;
                opisa.Location = new Point(producent[0].Location.X + producent[0].Width + 5, 15);
                opisa.Text = "Opis problemu";
                opisa.BorderStyle = BorderStyle.FixedSingle;

                idWlasciciela = new Label[1];
                idWlasciciela[0] = new Label();
                idWlasciciela[0].Width = 70;
                idWlasciciela[0].Height = 30;
                idWlasciciela[0].Location = new Point(opisa.Width + opisa.Location.X + 5, 15);
                idWlasciciela[0].Text = "Id właściciela";
                idWlasciciela[0].BorderStyle = BorderStyle.FixedSingle;

                status = new Label[1];
                status[0] = new Label();
                status[0].Width = 150;
                status[0].Height = 30;
                status[0].Location = new Point(idWlasciciela[0].Width + idWlasciciela[0].Location.X + 5, 15);
                status[0].Text = "Status naprawy";
                status[0].BorderStyle = BorderStyle.FixedSingle;

                dataPrzyjecia = new Label[1];
                dataPrzyjecia[0] = new Label();
                dataPrzyjecia[0].Width = 150;
                dataPrzyjecia[0].Height = 30;
                dataPrzyjecia[0].Location = new Point(status[0].Width + status[0].Location.X + 5, 15);
                dataPrzyjecia[0].Text = "Data przyjęcia";
                dataPrzyjecia[0].BorderStyle = BorderStyle.FixedSingle;

                dataOdbioru = new Label[1];
                dataOdbioru[0] = new Label();
                dataOdbioru[0].Width = 150;
                dataOdbioru[0].Height = 30;
                dataOdbioru[0].Location = new Point(dataPrzyjecia[0].Width + dataPrzyjecia[0].Location.X + 5, 15);
                dataOdbioru[0].Text = "Data odbioru";
                dataOdbioru[0].BorderStyle = BorderStyle.FixedSingle;

                idSprzetuE = new NumericUpDown();
                idSprzetuE.Width = 70;
                idSprzetuE.Height = 30;
                idSprzetuE.Location = new Point(10, 50);
                idSprzetuE.Value = 0;
                int maximum = -1;
                for (int i = 0; i < sprzet.Length; i++)
                {
                    if (sprzet[i] != null)
                    {
                        maximum++;
                    }
                    else
                    {
                        break;
                    }
                }
                idSprzetuE.Maximum = maximum;
                idSprzetuE.BorderStyle = BorderStyle.FixedSingle;
                idSprzetuE.Click += wyswietlEdytowanySprzet;

                typE = new RichTextBox();
                typE.Width = 150;
                typE.Height = 30;
                typE.Location = new Point(idSprzetuE.Location.X + idSprzetuE.Width + 5, 50);
                typE.Text = "";
                typE.BorderStyle = BorderStyle.FixedSingle;

                producentE = new RichTextBox();
                producentE.Width = 150;
                producentE.Height = 30;
                producentE.Location = new Point(typE.Width + typE.Location.X + 5, 50);
                producentE.Text = "";
                producentE.BorderStyle = BorderStyle.FixedSingle;

                opisE = new RichTextBox();
                opisE.Width = 150;
                opisE.Height = 30;
                opisE.Location = new Point(producentE.Location.X + producentE.Width + 5, 50);
                opisE.Text = "";
                opisE.Enabled = true;

                idWlascicielaE = new RichTextBox();
                idWlascicielaE.Width = 70;
                idWlascicielaE.Height = 30;
                idWlascicielaE.Location = new Point(opisE.Width + opisE.Location.X + 5, 50);
                idWlascicielaE.Text = "";
                idWlascicielaE.BorderStyle = BorderStyle.FixedSingle;

                statusE.Width = 150;
                statusE.Height = 30;
                statusE.Location = new Point(idWlascicielaE.Width + idWlascicielaE.Location.X + 5, 50);
                statusE.Text = "";

                dataPrzyjeciaE = new RichTextBox();
                dataPrzyjeciaE.Width = 150;
                dataPrzyjeciaE.Height = 30;
                dataPrzyjeciaE.Location = new Point(statusE.Width + statusE.Location.X + 5, 50);
                dataPrzyjeciaE.Text = "Data przyjęcia";
                dataPrzyjeciaE.BorderStyle = BorderStyle.FixedSingle;
                dataPrzyjeciaE.Enabled = false;

                dataOdbioruE = new RichTextBox();
                dataOdbioruE.Width = 150;
                dataOdbioruE.Height = 30;
                dataOdbioruE.Location = new Point(dataPrzyjeciaE.Width + dataPrzyjeciaE.Location.X + 5, 50);
                dataOdbioruE.Text = "Data odbioru";
                dataOdbioruE.BorderStyle = BorderStyle.FixedSingle;
                dataOdbioruE.Enabled = false;

                Akceptuj = null;
                Akceptuj = new Button();
                Akceptuj.Text = "Akceptuj";
                Akceptuj.Location = new Point(10, statusE.Location.Y + statusE.Height + 5);
                Akceptuj.Click += zmodyfikujSp;

                prawy.Controls.Add(idSprzetu[0]);
                prawy.Controls.Add(typ[0]);
                prawy.Controls.Add(producent[0]);
                prawy.Controls.Add(opisa);
                prawy.Controls.Add(idWlasciciela[0]);
                prawy.Controls.Add(status[0]);
                prawy.Controls.Add(dataOdbioru[0]);
                prawy.Controls.Add(dataPrzyjecia[0]);
                prawy.Controls.Add(idSprzetuE);
                prawy.Controls.Add(typE);
                prawy.Controls.Add(producentE);
                prawy.Controls.Add(opisE);
                prawy.Controls.Add(idWlascicielaE);
                prawy.Controls.Add(statusE);
                prawy.Controls.Add(dataOdbioruE);
                prawy.Controls.Add(dataPrzyjeciaE);
                prawy.Controls.Add(Akceptuj);
            }
            else
            {
                idSprzetu = new Label[1];
                idSprzetu[0] = new Label();
                idSprzetu[0].Width = 150;
                idSprzetu[0].Height = 30;
                idSprzetu[0].Location = new Point(10, 15);
                idSprzetu[0].Text = "Brak sprzętów w bazie.";
                idSprzetu[0].BorderStyle = BorderStyle.FixedSingle;

                prawy.Controls.Add(idSprzetu[0]);
            }

            Zmodyfikuj.Click -= zmodyfikujSprzet;
        }

        private void zmodyfikujSp(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Czy napewno zmodyfikować?","", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (klient[Convert.ToInt32(idWlascicielaE.Text)] != null)
                {
                    if (klient[Convert.ToInt32(idWlascicielaE.Text)].aktywny)
                    {
                        sprzet[Convert.ToInt32(idSprzetuE.Value)].typ = typE.Text;
                        sprzet[Convert.ToInt32(idSprzetuE.Value)].producent = producentE.Text;
                        sprzet[Convert.ToInt32(idSprzetuE.Value)].opis = opisE.Text;
                        sprzet[Convert.ToInt32(idSprzetuE.Value)].idWlasciciela = Convert.ToInt32(idWlascicielaE.Text);
                        sprzet[Convert.ToInt32(idSprzetuE.Value)].status = statusE.Text;
                        if (statusE.Text=="Odebrane")
                        {
                            sprzet[Convert.ToInt32(idSprzetuE.Value)].dataOdbioru = DateTime.Today.ToShortDateString() + "";
                        }
                        imie = new Label[1];
                        imie[0] = new Label();
                        imie[0].Width = 150;
                        imie[0].Height = 30;
                        imie[0].Location = new Point(10, 15);
                        imie[0].Text = "Poprawnie zmodyfikowano dane sprzętu";
                        imie[0].BorderStyle = BorderStyle.FixedSingle;
                        prawy.Controls.Clear();
                        prawy.Controls.Add(imie[0]);
                        zapisDoPliku();
                    }
                    else
                    {
                        MessageBox.Show("Prosze wprowadzić poprawne dane.");
                    }
                }
                else
                {
                    MessageBox.Show("Prosze wprowadzić poprawne dane.");
                }
            }
            else
            {
                imie = new Label[1];
                imie[0] = new Label();
                imie[0].Width = 150;
                imie[0].Height = 30;
                imie[0].Location = new Point(10, 15);
                imie[0].Text = "Nie zmodyfikowano danych sprzętu";
                imie[0].BorderStyle = BorderStyle.FixedSingle;
                prawy.Controls.Clear();
                prawy.Controls.Add(imie[0]);
            }
            Akceptuj.Click -= zmodyfikujSp;
        }

        private void wyswietlEdytowanySprzet(object sender, EventArgs e)
        {
            if (sprzet[Convert.ToInt32(idSprzetuE.Value)] != null)
            {
                idSprzetuE.Value = sprzet[Convert.ToInt32(idSprzetuE.Value)].idSprzetu;
                typE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].typ;
                producentE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].producent;
                opisE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].opis;
                idWlascicielaE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].idWlasciciela+"";
                statusE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].status;
                dataOdbioruE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].dataOdbioru;
                dataPrzyjeciaE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].dataPrzyjecia;
            }
            else
            {
                idSprzetuE.Value = 0;
                MessageBox.Show("Niepoprawny identyfikator sprzętu.");
            }
        }

        private void dodajSprzet(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            typ = new Label[1];
            typ[0] = new Label();
            typ[0].Width = 150;
            typ[0].Height = 30;
            typ[0].Location = new Point(10, 15);
            typ[0].Text = "Typ sprzętu";
            typ[0].BorderStyle = BorderStyle.FixedSingle;

            producent = new Label[1];
            producent[0] = new Label();
            producent[0].Width = 150;
            producent[0].Height = 30;
            producent[0].Location = new Point(typ[0].Width + typ[0].Location.X + 5, 15);
            producent[0].Text = "Nazwa producenta";
            producent[0].BorderStyle = BorderStyle.FixedSingle;

            Label opisa = new Label();
            opisa.Width = 150;
            opisa.Height = 30;
            opisa.Location = new Point(producent[0].Location.X + producent[0].Width + 5, 15);
            opisa.Text = "Opis problemu";
            opisa.BorderStyle = BorderStyle.FixedSingle;

            idWlasciciela = new Label[1];
            idWlasciciela[0] = new Label();
            idWlasciciela[0].Width = 70;
            idWlasciciela[0].Height = 30;
            idWlasciciela[0].Location = new Point(opisa.Width + opisa.Location.X + 5, 15);
            idWlasciciela[0].Text = "Id właściciela";
            idWlasciciela[0].BorderStyle = BorderStyle.FixedSingle;

            status = new Label[1];
            status[0] = new Label();
            status[0].Width = 150;
            status[0].Height = 30;
            status[0].Location = new Point(idWlasciciela[0].Width + idWlasciciela[0].Location.X + 5, 15);
            status[0].Text = "Status naprawy";
            status[0].BorderStyle = BorderStyle.FixedSingle;

            id = new Label[1];
            id[0] = new Label();
            id[0].Width = 70;
            id[0].Height = 30;
            id[0].Location = new Point(status[0].Width + status[0].Location.X + 5, 15);
            id[0].Text = "Serwisant";
            id[0].BorderStyle = BorderStyle.FixedSingle;

            koszt = new Label[1];
            koszt[0] = new Label();
            koszt[0].Width = 150;
            koszt[0].Height = 30;
            koszt[0].Location = new Point(id[0].Width + id[0].Location.X + 5, 15);
            koszt[0].Text = "Przewidywany koszt";
            koszt[0].BorderStyle = BorderStyle.FixedSingle;

            typE = new RichTextBox();
            typE.Width = 150;
            typE.Height = 30;
            typE.Location = new Point(10, 50);
            typE.Text = "";
            typE.BorderStyle = BorderStyle.FixedSingle;

            producentE = new RichTextBox();
            producentE.Width = 150;
            producentE.Height = 30;
            producentE.Location = new Point(typE.Width + typE.Location.X + 5, 50);
            producentE.Text = "";
            producentE.BorderStyle = BorderStyle.FixedSingle;

            opisE = new RichTextBox();
            opisE.Width = 150;
            opisE.Height = 30;
            opisE.Location = new Point(producentE.Location.X + producentE.Width + 5, 50);
            opisE.Text = "";
            opisE.Enabled = true;

            string item;
            idWlascicielaW = new ComboBox();
            for (int i = klient.Length -1 ; i >= 0; i--)
            {
                if (klient[i]!=null)
                {
                    item = klient[i].id + " - " + klient[i].nazwisko;
                    idWlascicielaW.Items.Add(item);
                }
            }
            idWlascicielaW.Width = 70;
            idWlascicielaW.Height = 30;
            idWlascicielaW.Location = new Point(opisE.Width + opisE.Location.X + 5, 50);
            idWlascicielaW.Text = "";
            
            statusE.Width = 150;
            statusE.Height = 30;
            statusE.Location = new Point(idWlascicielaW.Width + idWlascicielaW.Location.X + 5, 50);
            statusE.Text = "";

            nazwiskoSerwisanta = new ComboBox();
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i]!=null)
                {
                    if (pracownik[i].rola=="Administrator"||pracownik[i].rola=="Serwisant")
                    {
                        nazwiskoSerwisanta.Items.Add(pracownik[i].nazwisko);
                    }
                }
                else
                {
                    break;
                }
            }
            nazwiskoSerwisanta.Width = 70;
            nazwiskoSerwisanta.Height = 30;
            nazwiskoSerwisanta.Location = new Point(statusE.Width + statusE.Location.X + 5, 50);
            nazwiskoSerwisanta.Text = "";
            
            kosztE = new RichTextBox();
            kosztE.Width = 150;
            kosztE.Height = 30;
            kosztE.Location = new Point(nazwiskoSerwisanta.Width + nazwiskoSerwisanta.Location.X + 5, 50);
            kosztE.Text = "";
            kosztE.BorderStyle = BorderStyle.FixedSingle;

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, statusE.Location.Y + statusE.Height + 5);
            Akceptuj.Click += dodajSp;

            prawy.Controls.Add(typ[0]);
            prawy.Controls.Add(producent[0]);
            prawy.Controls.Add(opisa);
            prawy.Controls.Add(idWlasciciela[0]);
            prawy.Controls.Add(status[0]);
            prawy.Controls.Add(id[0]);
            prawy.Controls.Add(koszt[0]);
            prawy.Controls.Add(typE);
            prawy.Controls.Add(producentE);
            prawy.Controls.Add(opisE);
            prawy.Controls.Add(idWlascicielaW);
            prawy.Controls.Add(statusE);
            prawy.Controls.Add(nazwiskoSerwisanta);
            prawy.Controls.Add(kosztE);
            prawy.Controls.Add(Akceptuj);

            Dodaj.Click -= dodajSprzet;
        }

        private void dodajSp(object sender, EventArgs e)
        {
            int index = 0;
            for (int i = 0; i < sprzet.Length; i++)
            {
                if (sprzet[i]!=null)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            if (typE.Text != "" && producentE.Text != "" && opisE.Text != "" && idWlascicielaW.Text != "" && statusE.Text != "")
            {
                string idw = "";
                for (int i = 0; i < idWlascicielaW.Text.Length; i++)
                {
                    if (idWlascicielaW.Text[i] != ' ')
                    {
                        idw += idWlascicielaW.Text[i];
                    }
                    else
                    {
                        break;
                    }
                }
                if (klient[Convert.ToInt32(idw)] != null)
                {
                    if (klient[Convert.ToInt32(idw)].aktywny)
                    {
                        bool f = false;
                        string zamiana = kosztE.Text;
                        for (int i = 0; i < zamiana.Length; i++)
                        {
                            if (zamiana[i] == ',' || zamiana[i] == '0' || zamiana[i] == '1' || zamiana[i] == '2' || zamiana[i] == '3' || zamiana[i] == '4' || zamiana[i] == '5' || zamiana[i] == '6' || zamiana[i] == '7' || zamiana[i] == '8' || zamiana[i] == '9')
                            {
                                f = true;
                            }
                            else
                            {
                                f = false;
                                break;
                            }
                        }
                        if (f)
                        {
                            sprzet[index] = new sprzety(index, typE.Text, producentE.Text, opisE.Text, Convert.ToInt32(idw), statusE.Text, DateTime.Today.ToShortDateString(), "-");
                            serwis[index] = new serwisy(index, nazwiskoSerwisanta.Text, opisE.Text, Convert.ToDouble(kosztE.Text));
                            imie = new Label[1];
                            imie[0] = new Label();
                            imie[0].Width = 150;
                            imie[0].Height = 30;
                            imie[0].Location = new Point(10, 15);
                            imie[0].Text = "Poprawnie dodano dane sprzętu";
                            imie[0].BorderStyle = BorderStyle.FixedSingle;
                            prawy.Controls.Clear();
                            prawy.Controls.Add(imie[0]);
                            Akceptuj.Click -= dodajSp;
                            zapisDoPliku();
                        }
                        else
                        {
                            MessageBox.Show("Prosze podać poprawny format ceny.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Prosze wprowadzić poprawne dane.");
                    }
                }
                else
                {
                    MessageBox.Show("Prosze wprowadzić poprawne dane.");
                }
            }
            else
            {
                MessageBox.Show("Prosze wypełnić wszystkie pola.");
            }
        }

        private void wyswietlanieSerwisow(object sender, EventArgs e)
        {
            budujBazeSerwisow();
        }

        private void budujBazeSerwisow()
        {
            prawy.Controls.Clear();
            id = new Label[serwis.Length + 1];
            idSprzetu = new Label[serwis.Length + 1];
            status = new Label[serwis.Length + 1];
            koszt = new Label[serwis.Length + 1];
            opis = new RichTextBox[serwis.Length + 1];
            int j = serwis.Length - 1;
            int pozycjonowanie = 1;
            for (int i = 0; i <= serwis.Length; i++)
            {
                if (i == 0)
                {
                    id[i] = new Label();
                    id[i].Width = 70;
                    id[i].Height = 30;
                    id[i].Location = new Point(10, 10 + id[i].Height * i + 5);
                    id[i].Text = "Serwisant";
                    id[i].BorderStyle = BorderStyle.FixedSingle;

                    idSprzetu[i] = new Label();
                    idSprzetu[i].Width = 70;
                    idSprzetu[i].Height = 30;
                    idSprzetu[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * i + 5);
                    idSprzetu[i].Text = "Id Sprzętu";
                    idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                    Label opisa = new Label();
                    opisa.Width = 150;
                    opisa.Height = 30;
                    opisa.Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * i + 5);
                    opisa.Text = "Opis problemu";
                    opisa.BorderStyle = BorderStyle.FixedSingle;

                    status[i] = new Label();
                    status[i].Width = 150;
                    status[i].Height = 30;
                    status[i].Location = new Point(opisa.Width + opisa.Location.X + 5, 10 + opisa.Height * i + 5);
                    status[i].Text = "Status";
                    status[i].BorderStyle = BorderStyle.FixedSingle;

                    koszt[i] = new Label();
                    koszt[i].Width = 150;
                    koszt[i].Height = 30;
                    koszt[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * i + 5);
                    koszt[i].Text = "Koszt";
                    koszt[i].BorderStyle = BorderStyle.FixedSingle;

                    prawy.Controls.Add(id[i]);
                    prawy.Controls.Add(idSprzetu[i]);
                    prawy.Controls.Add(opisa);
                    prawy.Controls.Add(status[i]);
                    prawy.Controls.Add(koszt[i]);
                }
                else
                {
                    if (serwis[j + 1] != null)
                    {
                        if ((zalogowany.nazwisko == serwis[j + 1].nazwiskoSerwisanta || zalogowany.rola == "Administrator" || zalogowany.rola == "Biuro"))
                        {
                            id[i] = new Label();
                            id[i].Width = 70;
                            id[i].Height = 30;
                            id[i].Location = new Point(10, 10 + id[i].Height * pozycjonowanie + 5);
                            id[i].Text = serwis[j + 1].nazwiskoSerwisanta;
                            id[i].BorderStyle = BorderStyle.FixedSingle;

                            idSprzetu[i] = new Label();
                            idSprzetu[i].Width = 70;
                            idSprzetu[i].Height = 30;
                            idSprzetu[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * pozycjonowanie + 5);
                            idSprzetu[i].Text = serwis[j + 1].idSprzetu + "";
                            idSprzetu[i].BorderStyle = BorderStyle.FixedSingle;

                            opis[i] = new RichTextBox();
                            opis[i].Width = 150;
                            opis[i].Height = 30;
                            opis[i].Location = new Point(idSprzetu[i].Location.X + idSprzetu[i].Width + 5, 10 + idSprzetu[i].Height * pozycjonowanie + 5);
                            for (int k = 0; k < sprzet.Length; k++)
                            {
                                if (serwis[j + 1] != null && sprzet[k] != null)
                                    if (serwis[j + 1].idSprzetu == sprzet[k].idSprzetu)
                                    {
                                        opis[i].Text = sprzet[k].opis;
                                    }
                            }
                            

                            status[i] = new Label();
                            status[i].Width = 150;
                            status[i].Height = 30;
                            status[i].Location = new Point(opis[i].Width + opis[i].Location.X + 5, 10 + opis[i].Height * pozycjonowanie + 5);

                            for (int k = 0; k < sprzet.Length; k++)
                            {
                                if (serwis[j + 1] != null && sprzet[k] != null)
                                    if (serwis[j + 1].idSprzetu == sprzet[k].idSprzetu)
                                    {
                                        status[i].Text = sprzet[k].status;
                                    }
                            }
                            status[i].BorderStyle = BorderStyle.FixedSingle;

                            koszt[i] = new Label();
                            koszt[i].Width = 150;
                            koszt[i].Height = 30;
                            koszt[i].Location = new Point(status[i].Width + status[i].Location.X + 5, 10 + status[i].Height * pozycjonowanie + 5);
                            koszt[i].Text = serwis[j + 1].koszt + "";
                            koszt[i].BorderStyle = BorderStyle.FixedSingle;

                            prawy.Controls.Add(id[i]);
                            prawy.Controls.Add(idSprzetu[i]);
                            prawy.Controls.Add(opis[i]);
                            prawy.Controls.Add(status[i]);
                            prawy.Controls.Add(koszt[i]);
                            pozycjonowanie++;
                        }
                    }
                }
                j--;
            }
        }

        private void zarzadzanieSerwisami(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            Zmodyfikuj = new Button();
            Zmodyfikuj.Text = "Zmodyfikuj dane serwisu";
            Zmodyfikuj.Location = new Point(10, 15);
            Zmodyfikuj.Click += zmodyfikujSerwis;
            
            prawy.Controls.Add(Zmodyfikuj);
        }

        private void zmodyfikujSerwis(object sender, EventArgs e)
        {
            prawy.Controls.Clear();
            int ilserwisow = 0;
            for (int i = 0; i < serwis.Length; i++)
            {
                if (serwis[i] != null)
                {
                    if (zalogowany.nazwisko == serwis[i].nazwiskoSerwisanta)
                    {
                        ilserwisow++;
                    }
                }
                else
                {
                    break;
                }
            }
            if (ilserwisow > 0)
            {
                if (zalogowany.rola == "Administrator" || zalogowany.rola == "Biuro")
                {

                    id = new Label[1];
                    idSprzetu = new Label[1];
                    status = new Label[1];
                    koszt = new Label[1];
                    opis = new RichTextBox[1];

                    id[0] = new Label();
                    id[0].Width = 70;
                    id[0].Height = 30;
                    id[0].Location = new Point(10, 15);
                    id[0].Text = "Serwisant";
                    id[0].BorderStyle = BorderStyle.FixedSingle;

                    idSprzetu[0] = new Label();
                    idSprzetu[0].Width = 70;
                    idSprzetu[0].Height = 30;
                    idSprzetu[0].Location = new Point(id[0].Location.X + id[0].Width + 5, 15);
                    idSprzetu[0].Text = "Id Sprzętu";
                    idSprzetu[0].BorderStyle = BorderStyle.FixedSingle;

                    Label opisa = new Label();
                    opisa.Width = 150;
                    opisa.Height = 30;
                    opisa.Location = new Point(idSprzetu[0].Location.X + idSprzetu[0].Width + 5, 15);
                    opisa.Text = "Opis problemu";
                    opisa.BorderStyle = BorderStyle.FixedSingle;

                    status[0] = new Label();
                    status[0].Width = 150;
                    status[0].Height = 30;
                    status[0].Location = new Point(opisa.Width + opisa.Location.X + 5, 15);
                    status[0].Text = "Status";
                    status[0].BorderStyle = BorderStyle.FixedSingle;

                    koszt[0] = new Label();
                    koszt[0].Width = 150;
                    koszt[0].Height = 30;
                    koszt[0].Location = new Point(status[0].Width + status[0].Location.X + 5, 15);
                    koszt[0].Text = "Koszt";
                    koszt[0].BorderStyle = BorderStyle.FixedSingle;

                    nazwiskoE = new RichTextBox();
                    nazwiskoE.Width = 70;
                    nazwiskoE.Height = 30;
                    nazwiskoE.Location = new Point(10, 50);
                    nazwiskoE.Text = "";
                    nazwiskoE.BorderStyle = BorderStyle.FixedSingle;
                    if (zalogowany.rola == "Administrator" || zalogowany.rola == "Biuro")
                    {
                        nazwiskoE.Enabled = true;
                    }
                    else
                    {
                        nazwiskoE.Enabled = false;
                    }


                    idSprzetuE = new NumericUpDown();
                    idSprzetuE.Width = 70;
                    idSprzetuE.Height = 30;
                    idSprzetuE.Location = new Point(nazwiskoE.Location.X + nazwiskoE.Width + 5, 50);
                    idSprzetuE.Value = 0;
                    int maximum = -1;
                    for (int i = 0; i < sprzet.Length; i++)
                    {
                        if (sprzet[i] != null)
                        {
                            maximum++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    idSprzetuE.Maximum = maximum;
                    idSprzetuE.BorderStyle = BorderStyle.FixedSingle;
                    if (idSprzetuE.Value >= 0)
                    {
                        idSprzetuE.Click += wyswietlEdytowanySerwis;
                    }

                    opisE = new RichTextBox();
                    opisE.Width = 150;
                    opisE.Height = 30;
                    opisE.Location = new Point(idSprzetuE.Location.X + idSprzetuE.Width + 5, 50);
                    opisE.Text = "";
                    opisE.BorderStyle = BorderStyle.FixedSingle;
                    
                    statusE.Width = 150;
                    statusE.Height = 30;
                    statusE.Location = new Point(opisE.Width + opisE.Location.X + 5, 50);
                    statusE.Text = "";

                    kosztE = new RichTextBox();
                    kosztE.Width = 150;
                    kosztE.Height = 30;
                    kosztE.Location = new Point(statusE.Width + statusE.Location.X + 5, 50);
                    kosztE.Text = "";
                    kosztE.BorderStyle = BorderStyle.FixedSingle;

                    Akceptuj = null;
                    Akceptuj = new Button();
                    Akceptuj.Text = "Akceptuj";
                    Akceptuj.Location = new Point(10, kosztE.Location.Y + kosztE.Height + 5);
                    Akceptuj.Click += zmodyfikujSe;

                    prawy.Controls.Add(id[0]);
                    prawy.Controls.Add(idSprzetu[0]);
                    prawy.Controls.Add(opisa);
                    prawy.Controls.Add(status[0]);
                    prawy.Controls.Add(koszt[0]);
                    prawy.Controls.Add(nazwiskoE);
                    prawy.Controls.Add(idSprzetuE);
                    prawy.Controls.Add(opisE);
                    prawy.Controls.Add(statusE);
                    prawy.Controls.Add(kosztE);
                    prawy.Controls.Add(Akceptuj);

                    Zmodyfikuj.Click -= zmodyfikujSerwis;
                }
            }
            else
            {
                imie = new Label[1];
                imie[0] = new Label();
                imie[0].Width = 150;
                imie[0].Height = 30;
                imie[0].Location = new Point(10, 15);
                imie[0].Text = "Brak dostępnych serwisów.";
                imie[0].BorderStyle = BorderStyle.FixedSingle;
                prawy.Controls.Clear();
                prawy.Controls.Add(imie[0]);
            }
        }

        private void zmodyfikujSe(object sender, EventArgs e)
        {
            DialogResult result;
            result = MessageBox.Show("Czy napewno zmodyfikować?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (serwis[Convert.ToInt32(idSprzetuE.Text)] != null && sprzet[Convert.ToInt32(idSprzetuE.Text)] != null)
                {
                    serwis[Convert.ToInt32(idSprzetuE.Value)].nazwiskoSerwisanta = nazwiskoE.Text;
                    serwis[Convert.ToInt32(idSprzetuE.Value)].idSprzetu = Convert.ToInt32(idSprzetuE.Value);
                    sprzet[Convert.ToInt32(idSprzetuE.Value)].opis = opisE.Text;
                    serwis[Convert.ToInt32(idSprzetuE.Value)].opis = opisE.Text;
                    sprzet[Convert.ToInt32(idSprzetuE.Value)].status = statusE.Text;
                    serwis[Convert.ToInt32(idSprzetuE.Value)].koszt = Convert.ToDouble(kosztE.Text);
                    imie = new Label[1];
                        imie[0] = new Label();
                        imie[0].Width = 150;
                        imie[0].Height = 30;
                        imie[0].Location = new Point(10, 15);
                        imie[0].Text = "Poprawnie zmodyfikowano dane sprzętu";
                        imie[0].BorderStyle = BorderStyle.FixedSingle;
                        prawy.Controls.Clear();
                        prawy.Controls.Add(imie[0]);
                        zapisDoPliku();
                }
                else
                {
                    MessageBox.Show("Prosze wprowadzić poprawne dane.");
                }
            }
            else
            {
                imie = new Label[1];
                imie[0] = new Label();
                imie[0].Width = 150;
                imie[0].Height = 30;
                imie[0].Location = new Point(10, 15);
                imie[0].Text = "Nie zmodyfikowano danych sprzętu";
                imie[0].BorderStyle = BorderStyle.FixedSingle;
                prawy.Controls.Clear();
                prawy.Controls.Add(imie[0]);
            }
            Akceptuj.Click -= zmodyfikujSe;
        }

        private void wyswietlEdytowanySerwis(object sender, EventArgs e)
        {
            if (idSprzetuE.Value>=0)
            {
                if (serwis[Convert.ToInt32(idSprzetuE.Value)] != null && sprzet[Convert.ToInt32(idSprzetuE.Value)] != null)
                {
                    if (serwis[Convert.ToInt32(idSprzetuE.Value)].nazwiskoSerwisanta == zalogowany.nazwisko || zalogowany.rola == "Administrator" || zalogowany.rola == "Biuro")
                    {
                        nazwiskoE.Text = serwis[Convert.ToInt32(idSprzetuE.Value)].nazwiskoSerwisanta;
                        idSprzetuE.Value = serwis[Convert.ToInt32(idSprzetuE.Value)].idSprzetu;
                        opisE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].opis;
                        statusE.Text = sprzet[Convert.ToInt32(idSprzetuE.Value)].status;
                        kosztE.Text = serwis[Convert.ToInt32(idSprzetuE.Value)].koszt + "";
                    }
                    else
                    {
                        while (serwis[Convert.ToInt32(idSprzetuE.Value)].nazwiskoSerwisanta != zalogowany.nazwisko)
                        {
                            if (serwis[Convert.ToInt32(idSprzetuE.Value)] != null && sprzet[Convert.ToInt32(idSprzetuE.Value)] != null)
                            {
                                if ((idSprzetuE.Value + 1) < idSprzetuE.Maximum)
                                {
                                    idSprzetuE.Value += 1;
                                }
                                else
                                {
                                    idSprzetuE.Value = 0;
                                }

                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < serwis.Length; i++)
                    {
                        if (serwis[i] != null && sprzet[i] != null)
                        {
                            idSprzetuE.Value = i;
                            break;
                        }
                    }

                    MessageBox.Show("Niepoprawny identyfikator sprzetu.");
                }
            }
            else
            {
                imie = new Label[1];
                imie[0] = new Label();
                imie[0].Width = 150;
                imie[0].Height = 30;
                imie[0].Location = new Point(10, 15);
                imie[0].Text = "Brak sprzętów do wyświetlenia";
                imie[0].BorderStyle = BorderStyle.FixedSingle;
                prawy.Controls.Clear();
                prawy.Controls.Add(imie[0]);
            }
        }

        private void wyswietlaniePracowników(object sender, EventArgs e)
        {
            budujBazePracownikow();
        }

        private void budujBazePracownikow()
        {
            prawy.Controls.Clear();
            id = new Label[pracownik.Length + 1];
            imie = new Label[pracownik.Length + 1];
            nazwisko = new Label[pracownik.Length + 1];
            loginPracownika = new Label[pracownik.Length + 1];
            hasloPracownika = new Label[pracownik.Length + 1];
            rola = new Label[pracownik.Length + 1];
            int pozycjonowanie = 1;
            for (int i = 0; i <= pracownik.Length; i++)
            {
                if (i == 0)
                {
                    id[i] = new Label();
                    id[i].Width = 25;
                    id[i].Height = 30;
                    id[i].Location = new Point(10, 10 + id[i].Height * i + 5);
                    id[i].Text = "Id";
                    id[i].BorderStyle = BorderStyle.FixedSingle;

                    imie[i] = new Label();
                    imie[i].Width = 150;
                    imie[i].Height = 30;
                    imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * i + 5);
                    imie[i].Text = "Imie";
                    imie[i].BorderStyle = BorderStyle.FixedSingle;

                    nazwisko[i] = new Label();
                    nazwisko[i].Width = 150;
                    nazwisko[i].Height = 30;
                    nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * i + 5);
                    nazwisko[i].Text = "Nazwisko";
                    nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                    loginPracownika[i] = new Label();
                    loginPracownika[i].Width = 150;
                    loginPracownika[i].Height = 30;
                    loginPracownika[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * i + 5);
                    loginPracownika[i].Text = "Login pracownika";
                    loginPracownika[i].BorderStyle = BorderStyle.FixedSingle;

                    hasloPracownika[i] = new Label();
                    hasloPracownika[i].Width = 150;
                    hasloPracownika[i].Height = 30;
                    hasloPracownika[i].Location = new Point(loginPracownika[i].Width + loginPracownika[i].Location.X + 5, 10 + loginPracownika[i].Height * i + 5);
                    hasloPracownika[i].Text = "Hasło pracownika";
                    hasloPracownika[i].BorderStyle = BorderStyle.FixedSingle;

                    rola[i] = new Label();
                    rola[i].Width = 150;
                    rola[i].Height = 30;
                    rola[i].Location = new Point(hasloPracownika[i].Width + hasloPracownika[i].Location.X + 5, 10 + hasloPracownika[i].Height * i + 5);
                    rola[i].Text = "Rola pracownika";
                    rola[i].BorderStyle = BorderStyle.FixedSingle;
                }
                else if (pracownik[i - 1] != null)
                {
                    if (pracownik[i - 1].aktywny)
                    {
                        id[i] = new Label();
                        id[i].Width = 25;
                        id[i].Height = 30;
                        id[i].Location = new Point(10, 10 + id[i].Height * pozycjonowanie + 5);
                        id[i].Text = pracownik[i - 1].id + "";
                        id[i].BorderStyle = BorderStyle.FixedSingle;

                        imie[i] = new Label();
                        imie[i].Width = 150;
                        imie[i].Height = 30;
                        imie[i].Location = new Point(id[i].Location.X + id[i].Width + 5, 10 + id[i].Height * pozycjonowanie + 5);
                        imie[i].Text = pracownik[i - 1].imie;
                        imie[i].BorderStyle = BorderStyle.FixedSingle;

                        nazwisko[i] = new Label();
                        nazwisko[i].Width = 150;
                        nazwisko[i].Height = 30;
                        nazwisko[i].Location = new Point(imie[i].Width + imie[i].Location.X + 5, 10 + id[i].Height * pozycjonowanie + 5);
                        nazwisko[i].Text = pracownik[i - 1].nazwisko;
                        nazwisko[i].BorderStyle = BorderStyle.FixedSingle;

                        loginPracownika[i] = new Label();
                        loginPracownika[i].Width = 150;
                        loginPracownika[i].Height = 30;
                        loginPracownika[i].Location = new Point(nazwisko[i].Width + nazwisko[i].Location.X + 5, 10 + nazwisko[i].Height * pozycjonowanie + 5);
                        loginPracownika[i].Text = pracownik[i - 1].login;
                        loginPracownika[i].BorderStyle = BorderStyle.FixedSingle;

                        hasloPracownika[i] = new Label();
                        hasloPracownika[i].Width = 150;
                        hasloPracownika[i].Height = 30;
                        hasloPracownika[i].Location = new Point(loginPracownika[i].Width + loginPracownika[i].Location.X + 5, 10 + loginPracownika[i].Height * pozycjonowanie + 5);
                        hasloPracownika[i].Text = pracownik[i - 1].haslo;
                        hasloPracownika[i].BorderStyle = BorderStyle.FixedSingle;

                        rola[i] = new Label();
                        rola[i].Width = 150;
                        rola[i].Height = 30;
                        rola[i].Location = new Point(hasloPracownika[i].Width + hasloPracownika[i].Location.X + 5, 10 + hasloPracownika[i].Height * pozycjonowanie + 5);
                        rola[i].Text = pracownik[i - 1].rola;
                        rola[i].BorderStyle = BorderStyle.FixedSingle;
                        pozycjonowanie++;

                    }
                }

                prawy.Controls.Add(id[i]);
                prawy.Controls.Add(imie[i]);
                prawy.Controls.Add(nazwisko[i]);
                prawy.Controls.Add(loginPracownika[i]);
                prawy.Controls.Add(hasloPracownika[i]);
                prawy.Controls.Add(rola[i]);
            }
        }

        private void zarzadzaniePracownikami(object sender, EventArgs e)
        {
            prawy.Controls.Clear();

            Dodaj = new Button();
            Dodaj.Location = new Point(10, 10);
            Dodaj.Text = "Dodaj nowego pracownika";
            Dodaj.Click += dodajPracownika;

            Zmodyfikuj = new Button();
            Zmodyfikuj.Text = "Zmodyfikuj dane pracownika";
            Zmodyfikuj.Location = new Point(10, Dodaj.Location.Y + Dodaj.Height + 5);
            Zmodyfikuj.Click += zmodyfikujPracownika;

            Usun = new Button();
            Usun.Text = "Usuń dane pracownika";
            Usun.Location = new Point(10, Zmodyfikuj.Location.Y + Dodaj.Height + 5);
            Usun.Click += usunPracownika;

            prawy.Controls.Add(Dodaj);
            prawy.Controls.Add(Zmodyfikuj);
            prawy.Controls.Add(Usun);
        }

        private void usunPracownika(object sender, EventArgs e)
        {
            prawy.Controls.Clear();
            id = new Label[pracownik.Length + 1];
            imie = new Label[pracownik.Length + 1];
            nazwisko = new Label[pracownik.Length + 1];
            loginPracownika = new Label[pracownik.Length + 1];
            hasloPracownika = new Label[pracownik.Length + 1];
            rola = new Label[pracownik.Length + 1];

            id[0] = new Label();
            id[0].Width = 150;
            id[0].Height = 30;
            id[0].Location = new Point(10, 15);
            id[0].Text = "Id";
            id[0].BorderStyle = BorderStyle.FixedSingle;

            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(id[0].Width + id[0].Location.X + 5, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            loginPracownika[0] = new Label();
            loginPracownika[0].Width = 150;
            loginPracownika[0].Height = 30;
            loginPracownika[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            loginPracownika[0].Text = "Login pracownika";
            loginPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            hasloPracownika[0] = new Label();
            hasloPracownika[0].Width = 150;
            hasloPracownika[0].Height = 30;
            hasloPracownika[0].Location = new Point(loginPracownika[0].Width + loginPracownika[0].Location.X + 5, 15);
            hasloPracownika[0].Text = "Hasło pracownika";
            hasloPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            rola[0] = new Label();
            rola[0].Width = 150;
            rola[0].Height = 30;
            rola[0].Location = new Point(hasloPracownika[0].Width + hasloPracownika[0].Location.X + 5, 15);
            rola[0].Text = "rola pracownika";
            rola[0].BorderStyle = BorderStyle.FixedSingle;

            idE = new NumericUpDown();
            idE.Width = 150;
            idE.Height = 30;
            idE.Location = new Point(10, 50);
            idE.BorderStyle = BorderStyle.FixedSingle;
            int maximum = -1;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i] != null)
                {
                    maximum++;
                }
                else
                {
                    break;
                }
            }
            idE.Maximum = maximum;
            idE.Click += wyswietlEdytowanegoPracownika;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(idE.Width + idE.Location.X + 5, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imieE.Width + imieE.Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            loginPracownikaE = new RichTextBox();
            loginPracownikaE.Width = 150;
            loginPracownikaE.Height = 30;
            loginPracownikaE.Location = new Point(nazwiskoE.Width + nazwiskoE.Location.X + 5, 50);
            loginPracownikaE.Text = "";
            loginPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            hasloPracownikaE = new RichTextBox();
            hasloPracownikaE.Width = 150;
            hasloPracownikaE.Height = 30;
            hasloPracownikaE.Location = new Point(loginPracownikaE.Width + loginPracownikaE.Location.X + 5, 50);
            hasloPracownikaE.Text = "";
            hasloPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            rolaE = new RichTextBox();
            rolaE.Width = 150;
            rolaE.Height = 30;
            rolaE.Location = new Point(hasloPracownikaE.Width + hasloPracownikaE.Location.X + 5, 50);
            rolaE.Text = "";
            rolaE.BorderStyle = BorderStyle.FixedSingle;

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, rolaE.Location.Y + rolaE.Height + 5);
            Akceptuj.Click += usunPr;

            prawy.Controls.Add(id[0]);
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(loginPracownika[0]);
            prawy.Controls.Add(hasloPracownika[0]);
            prawy.Controls.Add(rola[0]);
            prawy.Controls.Add(idE);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(loginPracownikaE);
            prawy.Controls.Add(hasloPracownikaE);
            prawy.Controls.Add(rolaE);
            prawy.Controls.Add(Akceptuj);
            
            Usun.Click -= usunPracownika;
        }

        private void usunPr(object sender, EventArgs e)
        {
            bool flaga = false;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i] != null)
                {
                    if (Convert.ToInt32(idE.Text) == pracownik[i].id)
                    {
                        flaga = true;
                    }
                }
                else
                {
                    break;
                }
            }
            if (flaga)
            {
                DialogResult result;
                result = MessageBox.Show("Czy napewno usunąć dane?", "", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes &&
                    pracownik[Convert.ToInt32(idE.Text)].rola != "Administrator")
                {
                    pracownik[Convert.ToInt32(idE.Text)].imie = "Dane usunięte";
                    pracownik[Convert.ToInt32(idE.Text)].nazwisko = "Dane usunięte";
                    pracownik[Convert.ToInt32(idE.Text)].login = "Dane usunięte";
                    pracownik[Convert.ToInt32(idE.Text)].haslo = "Dane usunięte";
                    pracownik[Convert.ToInt32(idE.Text)].rola = "Dane usunięte";
                    pracownik[Convert.ToInt32(idE.Text)].aktywny = false;

                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Poprawnie zmodyfikowano dane Pracownika";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                    zapisDoPliku();
                }
                else
                {
                    if (pracownik[Convert.ToInt32(idE.Text)].rola == "Administrator")
                    {
                        MessageBox.Show("Nie można usunąć Administratora.");
                    }
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Nie usunięto danych Pracownika.";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                }
            }
            else
            {
                    MessageBox.Show("Prosze podać poprawny numer id klienta.");
            }
            Akceptuj.Click -= usunPr;
        }

        private void zmodyfikujPracownika(object sender, EventArgs e)
        {
            prawy.Controls.Clear();
            id = new Label[pracownik.Length + 1];
            imie = new Label[pracownik.Length + 1];
            nazwisko = new Label[pracownik.Length + 1];
            loginPracownika = new Label[pracownik.Length + 1];
            hasloPracownika = new Label[pracownik.Length + 1];
            rola = new Label[pracownik.Length + 1];

            id[0] = new Label();
            id[0].Width = 150;
            id[0].Height = 30;
            id[0].Location = new Point(10, 15);
            id[0].Text = "Id";
            id[0].BorderStyle = BorderStyle.FixedSingle;

            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(id[0].Width + id[0].Location.X + 5, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            loginPracownika[0] = new Label();
            loginPracownika[0].Width = 150;
            loginPracownika[0].Height = 30;
            loginPracownika[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            loginPracownika[0].Text = "Login pracownika";
            loginPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            hasloPracownika[0] = new Label();
            hasloPracownika[0].Width = 150;
            hasloPracownika[0].Height = 30;
            hasloPracownika[0].Location = new Point(loginPracownika[0].Width + loginPracownika[0].Location.X + 5, 15);
            hasloPracownika[0].Text = "Hasło pracownika";
            hasloPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            rola[0] = new Label();
            rola[0].Width = 150;
            rola[0].Height = 30;
            rola[0].Location = new Point(hasloPracownika[0].Width + hasloPracownika[0].Location.X + 5, 15);
            rola[0].Text = "rola pracownika";
            rola[0].BorderStyle = BorderStyle.FixedSingle;

            idE = new NumericUpDown();
            idE.Width = 150;
            idE.Height = 30;
            idE.Location = new Point(10, 50);
            idE.BorderStyle = BorderStyle.FixedSingle;
            int maximum = -1;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i] != null)
                {
                    maximum++;
                }
                else
                {
                    break;
                }
            }
            idE.Maximum = maximum;
            idE.Click += wyswietlEdytowanegoPracownika;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(idE.Width + idE.Location.X + 5, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imieE.Width + imieE.Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            loginPracownikaE = new RichTextBox();
            loginPracownikaE.Width = 150;
            loginPracownikaE.Height = 30;
            loginPracownikaE.Location = new Point(nazwiskoE.Width + nazwiskoE.Location.X + 5, 50);
            loginPracownikaE.Text = "";
            loginPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            hasloPracownikaE = new RichTextBox();
            hasloPracownikaE.Width = 150;
            hasloPracownikaE.Height = 30;
            hasloPracownikaE.Location = new Point(loginPracownikaE.Width + loginPracownikaE.Location.X + 5, 50);
            hasloPracownikaE.Text = "";
            hasloPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            rolaPracownika = new ComboBox();
            rolaPracownika.Items.Add("Administrator");
            rolaPracownika.Items.Add("Serwisant");
            rolaPracownika.Items.Add("Biuro");
            rolaPracownika.Width = 150;
            rolaPracownika.Height = 30;
            rolaPracownika.Location = new Point(hasloPracownikaE.Width + hasloPracownikaE.Location.X + 5, 50);
            rolaPracownika.Text = "";

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, rolaPracownika.Location.Y + rolaPracownika.Height + 5);
            Akceptuj.Click += zmodyfikujPr;

            prawy.Controls.Add(id[0]);
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(loginPracownika[0]);
            prawy.Controls.Add(hasloPracownika[0]);
            prawy.Controls.Add(rola[0]);
            prawy.Controls.Add(idE);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(loginPracownikaE);
            prawy.Controls.Add(hasloPracownikaE);
            prawy.Controls.Add(rolaPracownika);
            prawy.Controls.Add(Akceptuj);

            Dodaj.Click -= zmodyfikujPracownika;
        }

        private void zmodyfikujPr(object sender, EventArgs e)
        {
            bool flaga = false;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i] != null)
                {
                    if (Convert.ToInt32(idE.Text) == pracownik[i].id)
                    {
                        flaga = true;
                    }
                }
                else
                {
                    break;
                }
            }
            if (flaga)
            {
                DialogResult result;
                result = MessageBox.Show("Czy napewno zmodyfikować?", "", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    pracownik[Convert.ToInt32(idE.Text)].imie = imieE.Text;
                    pracownik[Convert.ToInt32(idE.Text)].nazwisko = nazwiskoE.Text;
                    pracownik[Convert.ToInt32(idE.Text)].login = loginPracownikaE.Text;
                    pracownik[Convert.ToInt32(idE.Text)].haslo = hasloPracownikaE.Text;
                    pracownik[Convert.ToInt32(idE.Text)].rola = rolaPracownika.Text;
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Poprawnie zmodyfikowano dane Pracownika";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                    zapisDoPliku();
                }
                else
                {
                    imie = new Label[1];
                    imie[0] = new Label();
                    imie[0].Width = 150;
                    imie[0].Height = 30;
                    imie[0].Location = new Point(10, 15);
                    imie[0].Text = "Nie zmodyfikowano danych Klienta";
                    imie[0].BorderStyle = BorderStyle.FixedSingle;
                    prawy.Controls.Clear();
                    prawy.Controls.Add(imie[0]);
                }
            }
            else
            {
                MessageBox.Show("Prosze podać poprawny numer id klienta.");
            }
            Akceptuj.Click -= zmodyfikujPr;
        }

        private void wyswietlEdytowanegoPracownika(object sender, EventArgs e)
        {
            if (pracownik[Convert.ToInt32(idE.Value)] != null)
            {
                    imieE.Text = pracownik[Convert.ToInt32(idE.Value)].imie;
                    nazwiskoE.Text = pracownik[Convert.ToInt32(idE.Value)].nazwisko;
                    loginPracownikaE.Text = pracownik[Convert.ToInt32(idE.Value)].login;
                hasloPracownikaE.Text = pracownik[Convert.ToInt32(idE.Value)].haslo;
                rolaPracownika.Text = pracownik[Convert.ToInt32(idE.Value)].rola;
                if (rolaPracownika.Text == "Administrator") 
                {
                    rolaPracownika.Enabled = false;
                }
                else
                {
                    rolaPracownika.Enabled = true;
                }
            }
            else
            {
                idE.Value = 0;
                MessageBox.Show("Niepoprawny identyfikator klienta.");
            }
        }

        private void dodajPracownika(object sender, EventArgs e)
        {
            prawy.Controls.Clear();
            id = new Label[pracownik.Length + 1];
            imie = new Label[pracownik.Length + 1];
            nazwisko = new Label[pracownik.Length + 1];
            loginPracownika = new Label[pracownik.Length + 1];
            hasloPracownika = new Label[pracownik.Length + 1];
            rola = new Label[pracownik.Length + 1];

            imie[0] = new Label();
            imie[0].Width = 150;
            imie[0].Height = 30;
            imie[0].Location = new Point(10, 15);
            imie[0].Text = "Imie";
            imie[0].BorderStyle = BorderStyle.FixedSingle;

            nazwisko[0] = new Label();
            nazwisko[0].Width = 150;
            nazwisko[0].Height = 30;
            nazwisko[0].Location = new Point(imie[0].Width + imie[0].Location.X + 5, 15);
            nazwisko[0].Text = "Nazwisko";
            nazwisko[0].BorderStyle = BorderStyle.FixedSingle;

            loginPracownika[0] = new Label();
            loginPracownika[0].Width = 150;
            loginPracownika[0].Height = 30;
            loginPracownika[0].Location = new Point(nazwisko[0].Width + nazwisko[0].Location.X + 5, 15);
            loginPracownika[0].Text = "Login pracownika";
            loginPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            hasloPracownika[0] = new Label();
            hasloPracownika[0].Width = 150;
            hasloPracownika[0].Height = 30;
            hasloPracownika[0].Location = new Point(loginPracownika[0].Width + loginPracownika[0].Location.X + 5, 15);
            hasloPracownika[0].Text = "Hasło pracownika";
            hasloPracownika[0].BorderStyle = BorderStyle.FixedSingle;

            rola[0] = new Label();
            rola[0].Width = 150;
            rola[0].Height = 30;
            rola[0].Location = new Point(hasloPracownika[0].Width + hasloPracownika[0].Location.X + 5, 15);
            rola[0].Text = "rola pracownika";
            rola[0].BorderStyle = BorderStyle.FixedSingle;

            imieE = new RichTextBox();
            imieE.Width = 150;
            imieE.Height = 30;
            imieE.Location = new Point(10, 50);
            imieE.Text = "";
            imieE.BorderStyle = BorderStyle.FixedSingle;

            nazwiskoE = new RichTextBox();
            nazwiskoE.Width = 150;
            nazwiskoE.Height = 30;
            nazwiskoE.Location = new Point(imieE.Width + imieE.Location.X + 5, 50);
            nazwiskoE.Text = "";
            nazwiskoE.BorderStyle = BorderStyle.FixedSingle;

            loginPracownikaE = new RichTextBox();
            loginPracownikaE.Width = 150;
            loginPracownikaE.Height = 30;
            loginPracownikaE.Location = new Point(nazwiskoE.Width + nazwiskoE.Location.X + 5, 50);
            loginPracownikaE.Text = "";
            loginPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            hasloPracownikaE = new RichTextBox();
            hasloPracownikaE.Width = 150;
            hasloPracownikaE.Height = 30;
            hasloPracownikaE.Location = new Point(loginPracownikaE.Width + loginPracownikaE.Location.X + 5, 50);
            hasloPracownikaE.Text = "";
            hasloPracownikaE.BorderStyle = BorderStyle.FixedSingle;

            rolaPracownika = new ComboBox();
            rolaPracownika.Items.Add("Administrator");
            rolaPracownika.Items.Add("Serwisant");
            rolaPracownika.Items.Add("Biuro");
            rolaPracownika.Width = 150;
            rolaPracownika.Height = 30;
            rolaPracownika.Location = new Point(hasloPracownikaE.Width + hasloPracownikaE.Location.X + 5, 50);
            rolaPracownika.Text = "";

            Akceptuj = null;
            Akceptuj = new Button();
            Akceptuj.Text = "Akceptuj";
            Akceptuj.Location = new Point(10, rolaPracownika.Location.Y + rolaPracownika.Height + 5);
            Akceptuj.Click += dodajPr;
            
            prawy.Controls.Add(imie[0]);
            prawy.Controls.Add(nazwisko[0]);
            prawy.Controls.Add(loginPracownika[0]);
            prawy.Controls.Add(hasloPracownika[0]);
            prawy.Controls.Add(rola[0]);
            prawy.Controls.Add(imieE);
            prawy.Controls.Add(nazwiskoE);
            prawy.Controls.Add(loginPracownikaE);
            prawy.Controls.Add(hasloPracownikaE);
            prawy.Controls.Add(rolaPracownika);
            prawy.Controls.Add(Akceptuj);

            Dodaj.Click -= dodajPracownika;
        }

        private void dodajPr(object sender, EventArgs e)
        {
            int index = 0;
            for (int i = 0; i < pracownik.Length; i++)
            {
                if (pracownik[i]!=null)
                {
                    index++;
                }
                else
                {
                    break;
                }
            }
            bool flaga = true;
            if (rolaPracownika.Text == "Administrator" || rolaPracownika.Text == "Serwisant" || rolaPracownika.Text == "Biuro")
            {
                for (int i = 0; i < pracownik.Length; i++)
                {
                    if (pracownik[i]!=null)
                    {
                        if (pracownik[i].login==loginPracownikaE.Text)
                        {
                            flaga = false;
                            MessageBox.Show("Prosze podać niepowtarzalny login.");
                            break;
                        }
                    }
                }
                if (flaga)
                {
                    pracownik[index] = new pracownicy(index, imieE.Text, nazwiskoE.Text, loginPracownikaE.Text, hasloPracownikaE.Text, rolaPracownika.Text, true);
                    prawy.Controls.Clear();
                    Akceptuj.Click -= dodajPr;
                }
            }
            else
            {
                MessageBox.Show("Prosze podać poprawne dane pracownika.");
            }
            zapisDoPliku();
        }

        private void wylogowanie(object sender, EventArgs e)
        {
            zalogowany = null;
            zapisDoPliku();
            budujPanelLogowania();
        }
    }
}

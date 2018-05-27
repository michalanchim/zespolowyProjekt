namespace Serwis
{
    public class pracownicy : osoby
    {
        public string login;
        public string haslo;
        public string rola;
        public bool aktywny;
        public pracownicy(int ida, string imiea, string nazwiskoa, string logina, string hasloa, string rolaa, bool aktywnya)
        {
            id = ida;
            imie = imiea;
            nazwisko = nazwiskoa;
            login = logina;
            haslo = hasloa;
            rola = rolaa;
            aktywny = aktywnya;
        }
    }
}
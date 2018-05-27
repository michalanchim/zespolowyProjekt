namespace Serwis
{
    public class klienci : osoby
    {
        public string nrtelefonu;
        public bool aktywny;
        public klienci(int ida, string imiea, string nazwiskoa, string nrtel, bool akty)
        {
            id = ida;
            imie = imiea;
            nazwisko = nazwiskoa;
            nrtelefonu = nrtel;
            aktywny = akty;
        }
    }
}
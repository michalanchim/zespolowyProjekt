namespace Serwis
{
    internal class serwisy
    {
        public int idSprzetu;
        public string nazwiskoSerwisanta;
        public string opis;
        public double koszt;
        public serwisy(int idsprz, string nazwisko,string op, double cena)
        {
            idSprzetu = idsprz;
            nazwiskoSerwisanta = nazwisko;
            opis = op;
            koszt = cena;
        }
    }
}
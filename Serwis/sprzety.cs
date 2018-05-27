namespace Serwis
{
    public class sprzety
    {
        public int idSprzetu;
        public string typ;
        public string producent;
        public string opis;
        public int idWlasciciela;
        public string status;
        public string dataPrzyjecia;
        public string dataOdbioru;
        public sprzety(int idSprz, string typs, string prod, string op, int idW, string stat, string datap, string datao)
        {
            idSprzetu = idSprz;
            typ = typs;
            producent = prod;
            opis = op;
            idWlasciciela = idW;
            status = stat;
            dataPrzyjecia = datap;
            dataOdbioru = datao;
        }
    }
}
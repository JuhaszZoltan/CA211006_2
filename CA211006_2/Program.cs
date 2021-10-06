using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA211006_2
{
    enum Marka { Peugeto, BMW, Mercedes, Ferrari, Lamborghini, }
    class Auto
    {
        private float _atlagFogyasztas;
        private int _tankMeret;
        private float _benzinMennyiseg;
        private string _rendszam;

        private static string valRszChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public float AtlagFogyasztas
        { 
            get => _atlagFogyasztas;
            set
            {
                if (value < 3) throw new Exception("túl alacsony átlagfogyasztás");
                if (value > 20) throw new Exception("túl magas átlagfogyasztás");
                _atlagFogyasztas = value;
            }
        }
        public int TankMeret 
        { 
            get => _tankMeret;
            set
            {
                if (value < 20) throw new Exception("túl kicsi tankméret");
                if (value > 100) throw new Exception("túl nagy tankméret");
                _tankMeret = value;
            }
        }
        public float BenzinMennyiseg 
        { 
            get => _benzinMennyiseg;
            set
            {
                if (value < 0) throw new Exception("nem lehet negatív");
                if (value > _tankMeret) throw new Exception("nem lehet nagyobb, mint a tank mérete");
                _benzinMennyiseg = value;
            }
        }
        public Marka Marka { get; set; }
        public bool AutopalyaMatrica { get; set; }
        public string Rendszam 
        { 
            get => _rendszam;
            set
            {
                bool elsoHarom = valRszChar.Contains(value[0]) && valRszChar.Contains(value[1]) && valRszChar.Contains(value[2]);
                bool kotojel = value[3] == '-';
                bool utolsoHarom = int.TryParse(value.Substring(4, 3), out _);
                bool hossz = value.Length == 7;
                if (!elsoHarom || !kotojel || !utolsoHarom || !hossz)
                    throw new Exception("nem megfelelő rendszám formátum");
                _rendszam = value;
            }
        }

        public float KmVanBenne => (BenzinMennyiseg / AtlagFogyasztas) * 100;

        public void GetAutoInfo()
        {
            Console.WriteLine($"{Rendszam} {Marka} {AtlagFogyasztas} liter/100Km tank: {BenzinMennyiseg}/{TankMeret} liter, matrica: {(AutopalyaMatrica ? "van" : "nincs")}");
        }

        public Auto(
            float atlagFogyasztas,
            int tankMeret,
            float benzinMennyiseg,
            Marka marka,
            bool autopalyaMatrica,
            string rendszam)
        {
            AtlagFogyasztas = atlagFogyasztas;
            TankMeret = tankMeret;
            BenzinMennyiseg = benzinMennyiseg;
            Marka = marka;
            AutopalyaMatrica = autopalyaMatrica;
            Rendszam = rendszam;
        }
        public Auto(string rendszam)
            : this(10F, 80, 5F, Marka.Lamborghini, false, rendszam) { }
    }

    class Program
    {
        static List<Auto> autok = new List<Auto>();
        static Random rnd = new Random();
        private static string betuk = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static void Main()
        {
            InitAutok();
            LegtobbetTudMegMenni();
            LegtobbMatricasMarka();
            Console.ReadKey();
        }
        private static void LegtobbMatricasMarka()
        {
            var dic = new Dictionary<Marka, int>();
            foreach (var a in autok)
            {
                if(a.AutopalyaMatrica)
                {
                    if (!dic.ContainsKey(a.Marka)) dic.Add(a.Marka, 1);
                    else dic[a.Marka]++;
                }
            }
            int maxValue = dic.Values.Max();
            Console.WriteLine($"A következő márkákból van a legtöbb ({maxValue} db) az autópályákon:");
            foreach (var kvp in dic)
                if (kvp.Value == maxValue) Console.WriteLine($"\t{kvp.Key}");

        }
        private static void LegtobbetTudMegMenni()
        {
            int maxi = 0;
            for (int i = 0; i < autok.Count; i++)
            {
                if (autok[i].KmVanBenne > autok[maxi].KmVanBenne) maxi = i;
            }
            Console.WriteLine($"legtöbb kilométert tud még menni a lista [{maxi}] indexű autója. adatai:");
            autok[maxi].GetAutoInfo();
        }
        private static void InitAutok()
        {
            for (int i = 0; i < 30; i++)
            {
                int tankMeret = rnd.Next(20, 101);
                autok.Add(new Auto(
                    atlagFogyasztas: rnd.Next(300, 2001) / 100F,
                    tankMeret: tankMeret,
                    benzinMennyiseg: rnd.Next(0, tankMeret),
                    marka: (Marka)rnd.Next(5),
                    autopalyaMatrica: rnd.Next(100) < 50,
                    rendszam: RendszamGen()));
            }
        }
        private static string RendszamGen()
        {
            return $"{betuk[rnd.Next(betuk.Length)]}{betuk[rnd.Next(betuk.Length)]}{betuk[rnd.Next(betuk.Length)]}-{rnd.Next(10)}{rnd.Next(10)}{rnd.Next(10)}";
        }
    }
}

using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace FinancialTracker.Views
{
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            LoadHelpContent();
        }

        private void LoadHelpContent()
        {
            try
            {
                // A súgó tartalom betöltése
                HelpDocument.Blocks.Clear();

                // Címek és fejezetek hozzáadása
                AddTitle("PÉNZÜGYI KÖVETŐ RENDSZER", 26);
                AddSubtitle("Felhasználói Kézikönyv", 18);

                AddHeading("TARTALOMJEGYZÉK", 20);
                AddBullet("1. Bevezetés");
                AddBullet("2. A Program Indítása");
                AddBullet("3. Főképernyő Áttekintése");
                AddBullet("4. Bevételek Kezelése");
                AddBullet("5. Kiadások Kezelése");
                AddBullet("6. Szűrés és Rendezés");
                AddBullet("7. Megtakarítások Kezelése");
                AddBullet("8. Kategória Beállítások");
                AddBullet("9. Gyakori Kérdések");
                AddBullet("10. Hibaelhárítás");

                AddHeading("BEVEZETÉS", 20);
                AddParagraph("A Pénzügyi Követő Rendszer egy egyszerű, de hatékony alkalmazás személyes pénzügyeid nyomon követésére. Segítségével könnyen kezelheted bevételeidet és kiadásaidat, kategóriákba sorolhatod költéseidet, és követheted megtakarításaidat. A program lehetővé teszi, hogy havi pénzügyi keretösszeget állíts be kategóriánként, ezzel is segítve a tudatos költekezést.");

                AddHeading("A PROGRAM INDÍTÁSA", 20);
                AddBullet("1. Az alkalmazás indításához kattints duplán a \"FinancialTracker.exe\" fájlra.");
                AddBullet("2. A program automatikusan betölti az előzőleg mentett adatokat, ha vannak ilyenek.");
                AddBullet("3. Ha első alkalommal használod a programot, alapértelmezett kategóriákkal és üres egyenleggel indul.");

                AddHeading("FŐKÉPERNYŐ ÁTTEKINTÉSE", 20);
                AddParagraph("A főképernyő két fő részből áll:");

                AddSubheading("Bal oldali panel:", 16);
                AddBullet("Fent láthatod a \"Bevétel\", \"Kiadás\" és \"Kategória beállítások\" gombokat.");
                AddBullet("Középen megjelenik az aktuális egyenleged.");
                AddBullet("Alatta látható a megtakarítási számlád egyenlege.");
                AddBullet("Az alsó részen a \"Kiadási lehetőségek\" táblázat mutatja, hogy kategóriánként mennyi pénzt költhetsz el az aktuális hónapban.");

                AddSubheading("Jobb oldali panel:", 16);
                AddBullet("Felül a kiadások listája található, ahol minden korábbi költésed megjelenik.");
                AddBullet("Alul a bevételek listája, ahol a bevételi tételeket láthatod.");
                AddBullet("Mindkét lista felett szűrési lehetőségek vannak.");

                AddHeading("BEVÉTELEK KEZELÉSE", 20);

                AddSubheading("Új bevétel hozzáadása:", 16);
                AddBullet("1. Kattints a \"Bevétel\" gombra a főképernyő bal felső részén.");
                AddBullet("2. A megjelenő ablakban add meg a bevétel adatait:");
                AddNestedBullet("Összeg: A bevétel összege forintban.");
                AddNestedBullet("Leírás: Rövid magyarázat a bevétel forrásáról.");
                AddNestedBullet("Dátum: A bevétel dátuma.");
                AddNestedBullet("Típus: Válaszd ki a megfelelő kategóriát (pl. Munkabér, Ösztöndíj, stb.).");
                AddNestedBullet("Megtakarítási százalék: Ha szeretnél automatikusan félretenni ebből a bevételből, állítsd be a százalékot.");
                AddBullet("3. Kattints a \"Mentés\" gombra.");

                AddSubheading("Bevétel törlése:", 16);
                AddBullet("1. A bevételek listájában keresd meg a törölni kívánt tételt.");
                AddBullet("2. Kattints a \"Törlés\" gombra a tétel sorában.");
                AddBullet("3. A megerősítő ablakban kattints az \"Igen\" gombra.");

                AddHeading("KIADÁSOK KEZELÉSE", 20);

                AddSubheading("Új kiadás hozzáadása:", 16);
                AddBullet("1. Kattints a \"Kiadás\" gombra a főképernyő bal felső részén.");
                AddBullet("2. A megjelenő ablakban add meg a kiadás adatait:");
                AddNestedBullet("Összeg: A kiadás összege forintban.");
                AddNestedBullet("Leírás: Rövid magyarázat a kiadás céljáról.");
                AddNestedBullet("Dátum: A kiadás dátuma.");
                AddNestedBullet("Kategória: Válaszd ki a megfelelő kategóriát (pl. Élelmiszer, Háztartás, stb.).");
                AddNestedBullet("Forrás: Válaszd ki, hogy egyenlegből vagy megtakarításból szeretnéd fizetni.");
                AddBullet("3. Kattints a \"Mentés\" gombra.");

                AddSubheading("Kiadás törlése:", 16);
                AddBullet("1. A kiadások listájában keresd meg a törölni kívánt tételt.");
                AddBullet("2. Kattints a \"Törlés\" gombra a tétel sorában.");
                AddBullet("3. A megerősítő ablakban kattints az \"Igen\" gombra.");

                AddHeading("SZŰRÉS ÉS RENDEZÉS", 20);

                AddSubheading("Kiadások szűrése:", 16);
                AddBullet("1. A kiadások lista felett válaszd ki a kategóriát a legördülő menüből a \"Típus\" mellett.");
                AddBullet("2. Válassz dátumot a \"Dátum\" mező mellett, ha egy adott napra szeretnél szűrni.");
                AddBullet("3. A szűrők törléséhez kattints a \"Szűrő törlés\" gombra.");

                AddSubheading("Listák rendezése:", 16);
                AddBullet("Kattints a lista fejlécében lévő oszlopnevekre (pl. \"Dátum\", \"Összeg\") a rendezéshez.");
                AddBullet("Ismételt kattintással váltogathatsz a növekvő és csökkenő sorrend között.");

                AddHeading("MEGTAKARÍTÁSOK KEZELÉSE", 20);
                AddBullet("1. A főképernyőn kattints a \"Megtakarítások\" gombra a bal oldali panelen.");
                AddBullet("2. A megjelenő oldalon láthatod a megtakarítási számlád egyenlegét.");
                AddBullet("3. Tájékozódhatsz a befektetési lehetőségekről és a részvényekről.");
                AddBullet("4. A \"Vissza\" gombbal térhetsz vissza a főképernyőre.");

                AddSubheading("Megtakarítás hozzáadása:", 16);
                AddBullet("Új bevétel hozzáadásakor beállíthatsz automatikus megtakarítási százalékot.");
                AddBullet("Közvetlenül is hozzáadhatsz megtakarítást, ha a \"Bevétel\" gombra kattintasz, és típusnak a \"Megtakarítás\" lehetőséget választod.");

                AddSubheading("Megtakarításból költés:", 16);
                AddBullet("Kiadás hozzáadásakor a \"Forrás\" legördülő menüben válaszd a \"Megtakarítás\" lehetőséget.");

                AddHeading("KATEGÓRIA BEÁLLÍTÁSOK", 20);
                AddBullet("1. A főképernyőn kattints a \"Kategória beállítások\" gombra.");
                AddBullet("2. A megjelenő ablakban beállíthatod, hogy az egyenleged hány százalékát szeretnéd az egyes kategóriákra fordítani.");
                AddBullet("3. A csúszkák segítségével vagy a számokat közvetlenül beírva állíthatod be a százalékos arányokat.");
                AddBullet("4. A beállítások összegének pontosan 100%-nak kell lennie a mentéshez.");
                AddBullet("5. Az \"Összesen\" érték alatt látható haladásjelző mutatja, hogy összesen hány százalékot állítottál be.");
                AddBullet("6. Ha kész vagy, kattints a \"Mentés\" gombra. Ha mégsem szeretnéd menteni a változtatásokat, kattints a \"Mégsem\" gombra.");

                AddSubheading("Hogyan működnek a kategória korlátok:", 16);
                AddBullet("Minden kategóriához beállíthatsz egy százalékos arányt.");
                AddBullet("A program az egyenleged alapján kiszámítja, hogy kategóriánként mennyi pénzt költhetsz el.");
                AddBullet("Ha egy kiadással túllépnéd a kategóriára beállított keretet, a program figyelmeztetést jelenít meg.");
                AddBullet("A főképernyőn a \"Kiadási lehetőségek\" listában láthatod, hogy az egyes kategóriákra mennyi pénzt fordíthatsz.");

                AddHeading("GYAKORI KÉRDÉSEK", 20);

                AddSubheading("1. Hogyan módosíthatom egy már meglévő kiadást vagy bevételt?", 16);
                AddParagraph("Sajnos a program jelenlegi verziójában nincs lehetőség a már meglévő tételek szerkesztésére. Ha hibásan vittél fel egy tételt, töröld ki, és add hozzá újra a helyes adatokkal.");

                AddSubheading("2. Mi történik, ha túllépem egy kategória költségvetési keretét?", 16);
                AddParagraph("A program figyelmeztetést jelenít meg, ha egy kiadással túllépnéd a kategóriára beállított keretet. Azonban a figyelmeztetés után még mindig hozzáadhatod a kiadást, ha szeretnéd.");

                AddSubheading("3. Hogyan állíthatok be automatikus megtakarítást?", 16);
                AddParagraph("Új bevétel hozzáadásakor a \"Megtakarítási százalék\" mezőben beállíthatod, hogy a bevétel hány százalékát szeretnéd automatikusan a megtakarítási számládra helyezni.");

                AddSubheading("4. Törölhetek egy kategóriát?", 16);
                AddParagraph("Nem, az alapértelmezett kategóriák nem törölhetők. Azonban módosíthatod a kategóriák százalékos arányait a \"Kategória beállítások\" menüben.");

                AddSubheading("5. Hol tárolódnak az adataim?", 16);
                AddParagraph("Az alkalmazás az adatokat a helyi gépen tárolja JSON formátumban. Az adatok a következő mappában találhatók:\nC:\\Users\\[felhasználónév]\\AppData\\Local\\FinancialTracker\\");

                AddHeading("HIBAELHÁRÍTÁS", 20);

                AddSubheading("A program nem indul el", 16);
                AddBullet("Ellenőrizd, hogy a számítógéped megfelel-e a rendszerkövetelményeknek.");
                AddBullet("Próbáld meg a programot rendszergazdaként futtatni.");

                AddSubheading("Az adataim eltűntek", 16);
                AddBullet("Ellenőrizd, hogy van-e mentett adat a C:\\Users\\[felhasználónév]\\AppData\\Local\\FinancialTracker\\ mappában.");
                AddBullet("Ha a fájlok sérültek, próbáld meg a programot újraindítani.");

                AddSubheading("Hibaüzenet jelenik meg", 16);
                AddBullet("Ha hibaüzenet jelenik meg, jegyezd fel a pontos szöveget.");
                AddBullet("Indítsd újra a programot, és próbáld elkerülni a hibát okozó műveletet.");
                AddBullet("Ha a hiba továbbra is fennáll, vedd fel a kapcsolatot a program fejlesztőjével.");

                // Lábléc
                AddFooter("A programot UKR készítette féléves projektként.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a súgó betöltésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTitle(string text, double fontSize)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddSubtitle(string text, double fontSize)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                FontSize = fontSize,
                FontWeight = FontWeights.SemiBold,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 0, 0, 30)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddHeading(string text, double fontSize)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(0, 102, 204)),
                Margin = new Thickness(0, 20, 0, 10)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddSubheading(string text, double fontSize)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                FontSize = fontSize,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(46, 139, 87)),
                Margin = new Thickness(0, 15, 0, 5)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddParagraph(string text)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                Margin = new Thickness(0, 0, 0, 10)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddBullet(string text)
        {
            Paragraph paragraph = new Paragraph(new Run("• " + text))
            {
                Margin = new Thickness(20, 0, 0, 5)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddNestedBullet(string text)
        {
            Paragraph paragraph = new Paragraph(new Run("  - " + text))
            {
                Margin = new Thickness(40, 0, 0, 5)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void AddFooter(string text)
        {
            Paragraph paragraph = new Paragraph(new Run(text))
            {
                FontStyle = FontStyles.Italic,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, 30, 0, 0)
            };
            HelpDocument.Blocks.Add(paragraph);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
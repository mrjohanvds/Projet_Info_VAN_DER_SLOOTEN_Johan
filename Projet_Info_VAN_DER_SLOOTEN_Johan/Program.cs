using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace Projet_Info_VAN_DER_SLOOTEN_Johan
{
    class Program
    {
        static void Main(string[] args)
        {           
            int boucle_du_programme = 0;
            while(boucle_du_programme == 0)
            {
                string myfile = "lac_en_montagne.bmp";

                MyImage MonImage = new MyImage(myfile);
                a:
                switch (Menu1())
                {
                    #region Projets

                    case 0:
                        b:                        
                        switch (Menu2_1())
                        {
                            case 0:

                                int ValueMenu1 = Menu2_1_1();
                                if (ValueMenu1 == 2) goto b;
                                MonImage.Projet_Nuance_de_Gris(ValueMenu1);
                                boucle_du_programme = MenuFin();
                                break;

                            case 1:

                                int ValueMenu2 = Menu2_1_2();
                                if (ValueMenu2 == 3) goto b;
                                MonImage.Projet_Rotation_De_LImage(ValueMenu2);
                                boucle_du_programme = MenuFin();
                                break;

                            case 2:

                                #region Changement de taille

                                int ValueMenu3 = Menu2_1_3();
                                int Coef = -1;
                                if(ValueMenu3 == 0)
                                {
                                    Console.Clear();
                                    Console.Write("Par combien voulez-vous agrandir votre image ?\n\nVeuillez inserez une valeur : ");
                                    Coef = int.Parse(Console.ReadLine());
                                }

                                if(ValueMenu3 == 1)
                                {
                                    Console.Clear();
                                    Console.Write("Par combien voulez-vous rétrécir votre image ?\n\nVeuillez inserez une valeur : ");
                                    Coef = int.Parse(Console.ReadLine());
                                }

                                if (ValueMenu3 == 2) goto b;
                                MonImage.Projet_Changement_Taille_De_LImage(ValueMenu3,Coef);
                                boucle_du_programme = MenuFin();
                                break;

                            #endregion

                            case 3:
                                int verif = 1;                                
                                string Name = "toto";
                                while(verif == 1)
                                {
                                    Console.Clear();
                                    Console.Write("Quel image voulez-vous superposer à l'image principale ?\nVeuillez inserez le nom de l'image (sans '.bmp'): ");
                                    Name = Console.ReadLine()+".bmp";
                                    verif = MenuVerif(Name);
                                }
                                MonImage.Projet_Superposition_Image(Name);
                                boucle_du_programme = MenuFin();
                                break;

                            case 4:

                                goto a;
                        }

                        Console.Clear();
                        Console.WriteLine("Votre image est enregristré sous le nom 'ImageModifiee'.");

                        break;

                    #endregion

                    #region Matrice de Convolution

                    case 1:
                        switch (Menu2_2())
                        {
                            case 0:
                                MonImage.Flou();
                                boucle_du_programme = MenuFin();
                                break;
                            case 1:
                                MonImage.Renforcement_des_bords();
                                boucle_du_programme = MenuFin();
                                break;
                            case 2:
                                MonImage.Detection_des_bords();
                                boucle_du_programme = MenuFin();
                                break;
                            case 3:
                                MonImage.Repoussage();
                                boucle_du_programme = MenuFin();
                                break;
                            case 4:
                                goto a;
                        }
                        break;

                    #endregion

                    #region Histogramme

                    case 2:

                        int ValueMenu = Menu2_3();
                        if (ValueMenu == 3) goto a;
                        MonImage.Histogramme(ValueMenu);
                        boucle_du_programme = MenuFin();
                        break;

                    #endregion

                    case 3:
                        MonImage.Initiale();
                        boucle_du_programme = MenuFin();
                        break;

                    #region Infos sur l'image

                    case 4:

                        switch (Menu2_4())
                        {
                            case 0:
                                Console.Clear();
                                MonImage.AfficherHeader();
                                Console.Write("\nAppuyez sur une touche pour continuer.");
                                Console.ReadKey();
                                boucle_du_programme = MenuFin();
                                break;

                            case 1:
                                Console.Clear();
                                Console.WriteLine(MonImage.toStringInfo());
                                Console.Write("\nAppuyez sur une touche pour continuer.");
                                Console.ReadKey();
                                boucle_du_programme = MenuFin();
                                break;

                            case 2:
                                Console.Clear();
                                MonImage.AfficherBytesImage();
                                Console.Write("\nAppuyez sur une touche pour continuer.");
                                Console.ReadKey();
                                boucle_du_programme = MenuFin();
                                break;

                            case 3:
                                goto a;
                                
                        }
                        break;

                    #endregion

                    case 5:

                        int ValueMenuInnov = MenuInnovation();
                        if (ValueMenuInnov == 5) goto a;
                        MonImage.Innovation(ValueMenuInnov);
                        boucle_du_programme = MenuFin();
                        break;

                    case 6:
                        boucle_du_programme = 1;
                        break;
                }
            }        
        }

        #region Menus

        static int Menu1()
        {
            int Ligne = 0;
            string[] Choix = { "-> Exécuter un projet.", "-> Matrice de convolution.", "-> Voir l'histogramme.", "-> Mon initiale.", "-> Voir les infos de votre image.", "-> Innovation","-> Quitter le programme." };
            
            Selection(Ligne, Choix);
            ConsoleKeyInfo cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int Menu2_1()
        {
            int Ligne = 0;
            string[] Choix = { "-> Nuance de gris.", "-> Rotation de l'image.", "-> Changement de taille de l'image.", "-> Superposition de deux images.", "-> Revenir au menu précedent." };

            Selection(Ligne, Choix);
            ConsoleKeyInfo cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int Menu2_1_1()
        {
            int Ligne = 0;
            string[] Choix = { "-> Transformer votre image en nuances de gris.", "-> Transfomez votre image en noir et blanc.", "-> Revenir au menu précedent." };
            ConsoleKeyInfo cki;
            Selection(Ligne, Choix);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();

            }
            return Ligne;
        }

        static int Menu2_1_2()
        {
            int Ligne = 0;
            string[] Choix = { "-> Tourner l'image de 90°", "-> Tourner l'image de 180°", "-> Tourner l'image de 270°", "-> Revenir au menu précedent." };

            Selection(Ligne, Choix);
            ConsoleKeyInfo cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int Menu2_1_3()
        {
            int Ligne = 0;
            string[] Choix = { "-> Agrandir l'image.", "-> Rétrécir l'image.", "-> Revenir au menu précedent." };
            ConsoleKeyInfo cki;
            Selection(Ligne, Choix);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();

            }
            return Ligne;
        }

        static int Menu2_2()
        {
            int Ligne = 0;
            string[] Choix = { "-> Flou", "-> Renforcement des bords", "-> Détection des bords", "-> Repoussage", "-> Revenir au menu précedent." };

            Selection(Ligne, Choix);
            ConsoleKeyInfo cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int Menu2_3()
        {
            int Ligne = 0;
            string[] Choix = { "-> Créer l'histogramme des pixels Rouges", "-> Créer l'histogramme des pixels Verts", "-> Créer l'histogramme des pixels Bleus", "-> Revenir au menu précedent." };

            Selection(Ligne, Choix);
            ConsoleKeyInfo cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int Menu2_4()
        {
            int Ligne = 0;
            string[] Choix = { "-> Voir bytes du HEADER et du INFOHEADER","-> Voir les caractéristiques de votre image.", "-> Voir votre image sous forme de bytes (Pour utilisateur confirmé, peut être TRES long et faire mal aux yeux).", "-> Revenir au menu précedent." };
            ConsoleKeyInfo cki;
            Selection(Ligne, Choix);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();

            }
            return Ligne;
        }

        static int MenuFin()
        {
            int Ligne = 0;
            string[] Choix = { "-> Revenir au menu principal.", "-> Fermer le programme."};
            ConsoleKeyInfo cki;
            Selection(Ligne, Choix);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                Selection(Ligne, Choix);
                cki = Console.ReadKey();

            }
            return Ligne;
        }

        static int MenuVerif(string Name)
        {
            int Ligne = 0;
            string[] Choix = { "-> Oui.", "-> Non." };
            ConsoleKeyInfo cki;
            SelectionVerif(Ligne, Choix,Name);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                SelectionVerif(Ligne, Choix,Name);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        static int MenuInnovation()
        {
            int Ligne = 0;
            int[] Choix = { 0, 1, 2, 3, 4, 5 };
            ConsoleKeyInfo cki;
            SelectionInnov(Ligne, Choix);
            cki = Console.ReadKey();

            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Ligne++;
                }
                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Ligne--;
                }
                if (Ligne == -1) Ligne = Choix.Length - 1;
                if (Ligne == Choix.Length) Ligne = 0;
                Console.Clear();
                SelectionInnov(Ligne, Choix);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        #endregion

        static void Selection(int Ligne, string[] Choix)
        {
            Console.Clear();
            Console.WriteLine("Que voulez-vous faire ?\n");
            for (int i = 0; i < Choix.Length; i++)
            {
                if (i == Choix.Length - 1) Console.WriteLine();
                if (Ligne == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(Choix[i]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\nUne fois votre choix sélectionné appuyez sur 'Enter'.");
        }

        static void SelectionVerif(int Ligne, string[] Choix, string Name)
        {
            Console.Clear();
            Console.WriteLine("Etes-vous sûr que le nom de votre fichier est : "+Name+" ?\n");
            for (int i = 0; i < Choix.Length; i++)
            {
                if (i == Choix.Length - 1) Console.WriteLine();
                if (Ligne == i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(Choix[i]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\nUne fois votre choix sélectionné appuyez sur 'Enter'.");
        }

        static void SelectionInnov(int Ligne, int[] Choix)
        {
            Console.Clear();
            Console.WriteLine("Quel drapeau voulez-vous supersposer à votre image ?\n");
            for (int i = 0; i < Choix.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;                

                #region France

                if (i == 0)
                {                
                    Console.BackgroundColor = ConsoleColor.Blue; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                    Console.BackgroundColor = ConsoleColor.Blue; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.Write(" ");
                    if(Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        
                    }
                    Console.WriteLine(" France");
                    Console.BackgroundColor = ConsoleColor.Blue; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                }

                #endregion

                #region Allemagne

                if (i == 1)
                {
                    Console.BackgroundColor = ConsoleColor.Black; Console.WriteLine("   ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.Write("   ");
                    if (Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    Console.WriteLine(" Allemagne");
                    Console.BackgroundColor = ConsoleColor.Yellow; Console.WriteLine("   ");
                }

                #endregion

                #region Italie

                if (i == 2)
                {
                    Console.BackgroundColor = ConsoleColor.Green; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                    Console.BackgroundColor = ConsoleColor.Green; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.Write(" ");
                    if (Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    Console.WriteLine(" Italie");
                    Console.BackgroundColor = ConsoleColor.Green; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.White; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                }


                #endregion

                #region Belgique

                if (i == 3)
                {
                    Console.BackgroundColor = ConsoleColor.Black; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Yellow; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                    Console.BackgroundColor = ConsoleColor.Black; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Yellow; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.Write(" ");
                    if (Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    Console.WriteLine(" Belgique");
                    Console.BackgroundColor = ConsoleColor.Black; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Yellow; Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine(" ");
                }

                #endregion

                if(i == 4)
                {
                    if (Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    Console.WriteLine("-> Personnalisez votre drapeau.");
                }

                if (i == Choix.Length - 1)
                {
                    if (Ligne == i)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    Console.WriteLine("\n-> Revenir au menu précedent.");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
            Console.WriteLine("\nUne fois votre choix sélectionné appuyez sur 'Enter'.");
        }
    }
}

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
    class MyImage
    {
        #region Attributs

        private byte[] myFile;
        private string nom_De_LImage;
        private byte[] header;
        private byte[] infoHeader;
        private RGB[,] image;
        private string type;
        private int tailleFichier;
        private int tailleOffset;
        private int largeur;
        private int hauteur;
        private int nbBitsCouleur;


        #endregion

        #region Propriétés

        public byte[] MyFile
        {
            get { return myFile; }
            set { myFile = value; }
        }

        public string Nom_De_LImage
        {
            get { return nom_De_LImage; }
            set { nom_De_LImage = value; }
        }

        public byte[] Header
        {
            get { return header; }
            set { header = value; }
        }

        public byte[] InfoHeader
        {
            get { return infoHeader; }
            set { infoHeader = value; }
        }

        public RGB[,] Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int TailleFichier
        {
            get { return tailleFichier; }
            set { tailleFichier = value; }
        }

        public int TailleOffset
        {
            get { return tailleOffset; }
            set { tailleOffset = value; }
        }

        public int Largeur
        {
            get { return largeur; }
            set { largeur = value; }
        }

        public int Hauteur
        {
            get { return hauteur; }
            set { hauteur = value; }
        }

        public int NbBitsCouleur
        {
            get { return nbBitsCouleur; }
            set { nbBitsCouleur = value; }
        }

        #endregion

        public MyImage(string file)
        {
            Nom_De_LImage = file;
            MyFile = File.ReadAllBytes(Nom_De_LImage);
            Header = DeterminerHeader();
            TailleOffset = DeterminerTailleOffset();
            InfoHeader = DeterminerInfoHeader();
            Type = DeterminerType();
            Largeur = DeterminerLargeur();
            Hauteur = DeterminerHauteur();
            TailleFichier = DeterminerTailleFichier();
            NbBitsCouleur = InfoHeader[14];
            Image = DeterminerImage();
        }


        /*-------------------------------------METHODES----------------------------------*/


        #region Methodes pour constructeur


        public byte[] DeterminerHeader()
        {
            byte[] Header = new byte[14];
            for (int i = 0; i < 14; i++)
                Header[i] = MyFile[i];
            return Header;
        }

        public byte[] DeterminerInfoHeader()
        {
            byte[] InfoHeader = new byte[TailleOffset - 14];
            for (int i = 14; i < TailleOffset; i++)
                InfoHeader[i - 14] = MyFile[i];
            return InfoHeader;
        }

        public RGB[,] DeterminerImage()
        {
            RGB[,] Image = new RGB[Hauteur, Largeur];
            int Indexi = 0;
            int Indexj = 0;
            for (int i = TailleOffset; i < MyFile.Length-2; i = i + 3)
            {
                if (Indexj == Largeur) { Indexj = 0; Indexi++; }
                byte[] MaCouleur = new byte[3];
                for (int j = 0; j < 3; j++) MaCouleur[j] = MyFile[i + j];
                RGB MonRGB = new RGB(MaCouleur);
                Image[Indexi, Indexj] = MonRGB;
                Indexj++;
            }
            return Image;
        }

        public string DeterminerType()
        {
            char[] ch = { Convert.ToChar(Header[0]), Convert.ToChar(Header[1]) }; //On convertit le code ASCII en char
            string str = new string(ch); //On convertit le tableau de char en string
            return str;
        }

        public int Convertir_Endian_To_Int(byte[] LittleEndian)
        {
            int EnInt = 0;
            for (int i = 0; i < 4; i++)
            {
                int value = 256;
                if (i == 0) value = 1;
                else
                {
                    if (i == 1) value = 256;
                    else
                    {
                        for (int index = 2; index <= i; index++) value = value * 256;
                    }
                }
                EnInt = EnInt + value * LittleEndian[i];
            }
            return EnInt;
        }

        public byte[] Convertir_Int_To_Endian(int Int)
        {
            int LSB = 0; int SSB = 0; int TSB = 0; int MSB = 0; int k = 0;
            byte[] EnEndian = { 0, 0, 0, 0 };

            #region Determination des bytes

            MSB = Int / 16777216;
            k = MSB * 16777216;
            if (Int % 16777216 != 0)
            {
                TSB = (Int - k) / 65536;
                if ((Int - k) % 65536 != 0)
                {
                    k = k + (TSB * 65536);
                    SSB = (Int - k) / 256;
                    if ((Int - k) % 256 != 0) k = k + (SSB * 256); LSB = Int - k;
                }
            }

            #endregion

            #region Insertion des bytes dans le tableau

            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        for (int j = 0; j < LSB; j++)
                        {
                            EnEndian[i]++;
                        }
                        break;

                    case 1:
                        for (int j = 0; j < SSB; j++)
                        {
                            EnEndian[i]++;
                        }
                        break;

                    case 2:
                        for (int j = 0; j < TSB; j++)
                        {
                            EnEndian[i]++;
                        }
                        break;

                    case 3:
                        for (int j = 0; j < MSB; j++)
                        {
                            EnEndian[i]++;
                        }
                        break;
                }
            }

            #endregion

            return EnEndian;
        }

        public int DeterminerLargeur()
        {
            byte[] bytesLargeur = { InfoHeader[4], InfoHeader[5], InfoHeader[6], InfoHeader[7] };
            int Largeur = Convertir_Endian_To_Int(bytesLargeur);
            return Largeur;
        }

        public int DeterminerHauteur()
        {
            byte[] bytesHauteur = { InfoHeader[8], InfoHeader[9], InfoHeader[10], InfoHeader[11] };
            int Hauteur = Convertir_Endian_To_Int(bytesHauteur);
            return Hauteur;
        }

        public int DeterminerTailleFichier()
        {
            byte[] bytesTaille = { Header[2], Header[3], Header[4], Header[5] };
            int Taille = Convertir_Endian_To_Int(bytesTaille);
            return Taille;
        }

        public int DeterminerTailleOffset()
        {
            byte[] bytesTaille = { Header[10], Header[11], Header[12], Header[13] };
            int Taille = Convertir_Endian_To_Int(bytesTaille);
            return Taille;
        }

        public string toStringInfo()
        {
            return "Cette image est de type " + Type + "."
                + "\nCette image fait : " + Largeur + " pixels de large et " + Hauteur + " pixels de haut."
                + "\nLa taille du fichier est de : " + TailleFichier + " octets."
                + "\nLa taille de l'Offset est de : " + TailleOffset + " octets."
                + "\nLe nombre bits par couleur est de : " + NbBitsCouleur + " bits.";
        }

        public void AfficherHeader()
        {
            Console.WriteLine("HEADER"); Console.WriteLine();Console.WriteLine();
            for (int i = 0; i < 14; i++)
                Console.Write(MyFile[i] + " ");
            Console.WriteLine("\n\nHEADER INFO\n\n");
            for (int i = 14; i < TailleOffset; i++)
                Console.Write(MyFile[i] + " ");
        }

        public void AfficherBytesImage()
        {
            Console.WriteLine("\n");
            for (int i = 0; i < Hauteur; i++)
            {

                for (int j = 0; j < Largeur; j++)
                {
                    Image[i, j].AfficherBytesCouleur();
                    Console.Write(". ");
                }
                Console.WriteLine();
            }
        }


        #endregion


        public void Image_To_File() 
        {
            for (int i = 4; i < 8; i++) InfoHeader[i] = Convertir_Int_To_Endian(Largeur)[i - 4];
            for (int i = 8; i < 12; i++) InfoHeader[i] = Convertir_Int_To_Endian(Hauteur)[i - 8];
            for (int i = 2; i < 6; i++) Header[i] = Convertir_Int_To_Endian(TailleFichier)[i - 2];
            byte[] ImageFinale = new byte[TailleFichier];
            for (int i = 0; i < Header.Length; i++)
            {
                ImageFinale[i] = Header[i];
            }
            for (int i = 0; i < InfoHeader.Length; i++)
            {
                ImageFinale[i + Header.Length] = InfoHeader[i];
            }
            int Compteur = 0;
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    for (int index = 0; index < 3; index++)
                    {
                        switch (index)
                        {
                            case 0:
                                ImageFinale[TailleOffset + Compteur] = Image[i, j].Rouge;
                                break;
                            case 1:
                                ImageFinale[TailleOffset + Compteur] = Image[i, j].Vert;
                                break;
                            case 2:
                                ImageFinale[TailleOffset + Compteur] = Image[i, j].Bleu;
                                break;
                        }
                        Compteur++;
                    }
                }
            }
            File.WriteAllBytes("ImageModifiee.bmp", ImageFinale);
        }


        #region Projets

        public void Projet_Nuance_de_Gris(int Choix)
        {
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    switch (Choix)
                    {
                        case 0:Image[i, j].NuanceDeGris();break;
                        case 1:Image[i, j].NoirEtBlanc();break;
                    }                    
                }
            }

            Image_To_File();
        }

        public void Projet_Rotation_De_LImage(int Angle)
        {            
            switch (Angle)
            {
                #region Case 1

                case 0:
                    RGB[,] ImageTournee90 = new RGB[Largeur, Hauteur];
                    for (int i = 0; i < Largeur; i++)
                        for (int j = 0; j < Hauteur; j++)
                        {
                            byte[] Noir = { 0, 0, 0 };
                            ImageTournee90[i, j] = new RGB(Noir);
                        }
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            ImageTournee90[j, Hauteur - i - 1].CloneCouleur(Image[i, j]);
                        }
                    }

                    Hauteur = ImageTournee90.GetLength(0);
                    Largeur = ImageTournee90.GetLength(1);

                    Image = ImageTournee90;


                    break;

                #endregion

                #region Case 2

                case 1:
                    if (Hauteur % 2 == 0)
                    {
                        for (int i = 0; i < Hauteur / 2; i++)
                        {
                            for (int j = 0; j < Largeur; j++)
                            {
                                byte[] Tab = { 0, 0, 0 };
                                RGB Stockage = new RGB(Tab);
                                Stockage.CloneCouleur(Image[i, j]);
                                Image[i, j].CloneCouleur(Image[Hauteur - i - 1, Largeur - j - 1]);
                                Image[Hauteur - i - 1, Largeur - j - 1].CloneCouleur(Stockage);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Hauteur; i++)
                        {
                            for (int j = 0; j < Largeur; j++)
                            {
                                RGB Stockage = Image[i, j];
                                Image[i, j].CloneCouleur(Image[Hauteur - i, Largeur - j]);
                                Image[Hauteur - i, Largeur - j].CloneCouleur(Image[i, j]);
                                if (i == Hauteur / 2 && j == Largeur / 2) break;
                            }
                        }
                    }
                    break;

                #endregion

                #region Case 3

                case 2:
                    RGB[,] ImageTournee270 = new RGB[Largeur, Hauteur];
                    for (int i = 0; i < Largeur; i++)
                        for (int j = 0; j < Hauteur; j++)
                        {
                            byte[] Noir = { 0, 0, 0 };
                            ImageTournee270[i, j] = new RGB(Noir);
                        }
                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            ImageTournee270[j,i].CloneCouleur(Image[i, Largeur - j - 1]);
                        }
                    }

                    Hauteur = ImageTournee270.GetLength(0);
                    Largeur = ImageTournee270.GetLength(1);

                    Image = ImageTournee270;
                    break;

                    #endregion
            }

            Image_To_File();
        }

        public void Projet_Changement_Taille_De_LImage(int Choix, int Coef)
        {
            switch (Choix)
            {
                #region Agrandissement

                case 0:
                    RGB[,] ImageAgrandie = new RGB[Hauteur * Coef, Largeur * Coef];
                    for (int i = 0; i < ImageAgrandie.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageAgrandie.GetLength(1); j++)
                        {
                            if (j % Coef == 0) ImageAgrandie[i, j] = Image[i / Coef, j / Coef];
                            else ImageAgrandie[i, j] = ImageAgrandie[i, j - 1];
                            if (i % Coef == 0) ImageAgrandie[i, j] = Image[i / Coef, j / Coef];
                            else ImageAgrandie[i, j] = ImageAgrandie[i - 1, j];
                        }
                    }
                    TailleFichier = (TailleFichier - TailleOffset) * Coef * Coef + TailleOffset;
                    Hauteur = Hauteur * Coef;
                    Largeur = Largeur * Coef;
                    Image = ImageAgrandie;
                    Image_To_File();
                    break;

                #endregion

                #region Rétrécissement

                case 1:
                    RGB[,] ImageRetrecie = new RGB[Hauteur / Coef, Largeur / Coef];
                    for (int i = 0; i < ImageRetrecie.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRetrecie.GetLength(1); j++)
                        {
                            int Rouge = 0;
                            int Vert = 0;
                            int Bleu = 0;
                            for(int index1 = i * Coef; index1 < i * Coef + Coef; index1++)
                            {
                                for (int index2 = j * Coef; index2 < j * Coef + Coef; index2++)
                                {
                                    Rouge += Image[index1, index2].Rouge;
                                    Vert += Image[index1, index2].Vert;
                                    Bleu += Image[index1, index2].Bleu;
                                }
                            }                            
                            byte[] Pixel = { Convert.ToByte(Rouge/(Coef * Coef)), Convert.ToByte(Vert / (Coef * Coef)), Convert.ToByte(Bleu / (Coef * Coef)) };
                            ImageRetrecie[i, j] = new RGB(Pixel);
                        }
                    }
                    Hauteur = Hauteur / Coef;
                    Largeur = Largeur / Coef;
                    TailleFichier = (TailleFichier-TailleOffset) /(Coef * Coef) +TailleOffset;
                    Image = ImageRetrecie;
                    Image_To_File();
                    break;

                    #endregion
            }
            
        }

        public void Projet_Superposition_Image(string Nom)
        {
            MyImage Image2 = new MyImage(Nom);
            RGB[,] ImageFinale = new RGB[Hauteur, Largeur];
            for(int i =0; i<Hauteur; i++)
            {
                if(i < Image2.Hauteur)
                {
                    for (int j = 0; j < Largeur; j++)
                    {
                        if (j < Image2.Largeur)
                        {
                            int Rouge = (Image[i, j].Rouge + Image2.Image[i, j].Rouge) / 2;
                            int Vert = (Image[i, j].Vert + Image2.Image[i, j].Vert) / 2;
                            int Bleu = (Image[i, j].Bleu + Image2.Image[i, j].Bleu) / 2;
                            byte[] Pixel = { Convert.ToByte(Rouge), Convert.ToByte(Vert), Convert.ToByte(Bleu) };
                            ImageFinale[i, j] = new RGB(Pixel);
                        }
                        else ImageFinale[i, j] = Image[i, j];
                    }
                }
                else
                {
                    for (int j = 0; j < Largeur; j++)
                    {
                            ImageFinale[i, j] = Image[i,j];
                    }
                }               
            }
            Image = ImageFinale;
            Image_To_File();
        }

        #endregion

        #region Matrice de convolution

        public void Flou()
        {
            int[,] Matrice = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            for (int i = 1; i < Hauteur - 1; i++)
            {
                for (int j = 1; j < Largeur - 1; j++)
                {
                    Image[i, j] = Multiplication_Matrice(i, j, Matrice);
                }
            }
            Image_To_File();
        }

        public void Renforcement_des_bords()
        {
            RGB[,] New_Image = new RGB[Hauteur, Largeur];
            int[,] Matrice = { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            for (int i = 0; i <Hauteur; i++)
            {
                for (int j = 0; j <Largeur; j++)
                {
                    byte[] Noir = { 0, 0, 0 };
                    New_Image[i, j] = new RGB(Noir);
                    if (j != 0 && j != Largeur - 1 && i != 0 && i != Hauteur - 1)
                    {
                        New_Image[i,j] = Multiplication_Matrice(i, j, Matrice);
                    }                                              
                }                                
            }            
            Image = New_Image;
            Image_To_File();
        }

        public void Detection_des_bords()
        {
            RGB[,] New_Image = new RGB[Hauteur, Largeur];
            int[,] Matrice = { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    byte[] Noir = { 0, 0, 0 };
                    New_Image[i, j] = new RGB(Noir);
                    if (j != 0 && j!= Largeur-1 && i != 0 && i != Hauteur - 1)
                    {
                        New_Image[i, j] = Multiplication_Matrice(i, j, Matrice);
                    }
                }
            }
            Image = New_Image;
            Image_To_File();
        }

        public void Repoussage()
        {
            RGB[,] New_Image = new RGB[Hauteur, Largeur];
            int[,] Matrice = { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    byte[] Noir = { 0, 0, 0 };
                    New_Image[i, j] = new RGB(Noir);
                    if (j != 0 && j != Largeur - 1 && i != 0 && i != Hauteur - 1)
                    {
                        New_Image[i, j] = Multiplication_Matrice(i, j, Matrice);
                    }
                }
            }
            Image = New_Image;
            Image_To_File();
        }

        public RGB Multiplication_Matrice(int i, int j, int[,]Matrice_Convolution)
        {
            int Compteur = 0;
            int Red = 0;
            int Green = 0;
            int Blue = 0;
            RGB[,] Matrice_Pixel = { {Image[i-1,j-1],Image[i-1,j],Image[i-1,j+1] },
                        {Image[i,j-1],Image[i,j],Image[i,j+1] },
                        {Image[i+1,j-1],Image[i+1,j],Image[i+1,j+1] } };

            for (int index1 = 0; index1 < Matrice_Convolution.GetLength(0); index1++)
            {
                for(int index2 = 0; index2 < Matrice_Convolution.GetLength(1); index2++)
                {
                    Red += Matrice_Convolution[index1, index2] * Matrice_Pixel[index1, index2].Rouge;
                    Green += Matrice_Convolution[index1, index2] * Matrice_Pixel[index1, index2].Vert;
                    Blue += Matrice_Convolution[index1, index2] * Matrice_Pixel[index1, index2].Bleu;
                    if (Matrice_Convolution[index1, index2] > 0) Compteur ++;
                }
            }
            byte[] Pixel = { Convert.ToByte(Math.Abs(Red / Compteur)), Convert.ToByte(Math.Abs(Green / Compteur)), Convert.ToByte(Math.Abs(Blue / Compteur)) };
            RGB New_RGB = new RGB(Pixel);
            return New_RGB;
        }

        #endregion

        public void Histogramme(int Choix)
        {
            int Maximum = 0;
            byte[] Noir = { 0, 0, 0 };
            byte[] Blanc = { 255, 255, 255 };
            int[] HistogrammeTab = new int[256];
            for(int i =0; i <HistogrammeTab.Length; i++)
            {
                HistogrammeTab[i] = 0;
            }
            for (int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                {
                    int value = 0;
                    if(Choix == 0)
                    {
                        value = Image[i, j].Rouge;
                    }
                    if (Choix == 1)
                    {
                        value = Image[i, j].Vert;
                    }
                    if (Choix == 2)
                    {
                        value = Image[i, j].Bleu;
                    }
                    HistogrammeTab[value]++;

                }
            }
            for(int i =0; i<HistogrammeTab.Length; i++)
            {
                if (HistogrammeTab[i] > Maximum) Maximum = HistogrammeTab[i];
            }
            int CoefLargeur = Maximum / 256;
            RGB[,] Histogramme = new RGB[Maximum, 256*CoefLargeur];
            for(int i = Histogramme.GetLength(0)-1; i >= 0; i--)
            {
                for(int j =0; j<Histogramme.GetLength(1); j+=CoefLargeur)
                {
                    for(int index =0; index < CoefLargeur; index++)
                    {
                        if (HistogrammeTab[j/CoefLargeur] > i) Histogramme[i, j+index] = new RGB(Blanc);
                        else Histogramme[i, j+index] = new RGB(Noir);
                    }
                }
            }
            Hauteur = Maximum;
            Largeur = Histogramme.GetLength(1);
            TailleFichier = Hauteur * Largeur * 3 + TailleOffset;
            Image = Histogramme;
            Projet_Changement_Taille_De_LImage(1, 2);
        }

        public void Initiale()
        {
            RGB[,] J = new RGB[5, 5];
            byte[] Noir = { 0, 0, 0 };
            byte[] Blanc = { 255, 255, 255 };

            for( int i = 0; i < 5; i++)
            {
                for(int j =0; j<5; j++)
                {
                    J[i, j] = new RGB(Blanc);
                    if (i == 0) J[i, j] = new RGB(Noir);
                    if (j == 2) J[i, j] = new RGB(Noir);
                    if(i == 5 && (j == 0 || j ==1)) J[i, j] = new RGB(Noir);
                }
            }
            Hauteur = 5;
            Largeur = 5;
            TailleFichier = 75 + TailleOffset;
            Image = J;
            Image_To_File();
        }

        public void Innovation(int Drapeau)
        {
            switch (Drapeau)
            {
                #region France

                case 0:
                    int LargeurZonesF = Largeur / 3;

                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (j < LargeurZonesF) Image[i, j].NuanceDeBleu();
                            else if (j > LargeurZonesF * 2) Image[i, j].NuanceDeRouge();
                            else Image[i, j].NuanceDeGris();
                        }
                    }

                    Image_To_File();
                    break;

                #endregion

                #region Allemagne

                case 1:
                    int HauteurZonesA = Hauteur / 3;

                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (i < HauteurZonesA) Image[i, j].NuanceDeJaune();
                            else if (i > HauteurZonesA * 2) Image[i, j].NuanceDeNoir();
                            else Image[i, j].NuanceDeRouge();
                        }
                    }

                    Image_To_File();
                    break;

                #endregion

                #region Italie

                case 2:
                    int LargeurZonesI = Largeur / 3;

                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (j < LargeurZonesI) Image[i, j].NuanceDeVert();
                            else if (j > LargeurZonesI * 2) Image[i, j].NuanceDeRouge();
                            else Image[i, j].NuanceDeGris();
                        }
                    }

                    Image_To_File();
                    break;

                #endregion

                #region Belgique

                case 3:
                    int LargeurZonesB = Largeur / 3;

                    for (int i = 0; i < Hauteur; i++)
                    {
                        for (int j = 0; j < Largeur; j++)
                        {
                            if (j < LargeurZonesB) Image[i, j].NuanceDeNoir();
                            else if (j > LargeurZonesB * 2) Image[i, j].NuanceDeRouge();
                            else Image[i, j].NuanceDeJaune();
                        }
                    }
                    Image_To_File();
                    break;

                #endregion

                #region Personnalisé

                case 4:
                    
                    switch (MenuPersonnalisation())
                    {
                        #region Vertical

                        case 0:
                            Console.Clear();
                            Console.WriteLine("Combien de bandes voulez-vous ?"); int NbBandesV = int.Parse(Console.ReadLine());
                            int LargeurBandesV = Largeur / NbBandesV;
                            for (int i =0; i < NbBandesV; i++)
                            {
                                int CouleurDeLaBandeV = MenuCouleur(i + 1);
                                for (int indexi = 0; indexi < Hauteur; indexi++)
                                {
                                    for (int indexj = LargeurBandesV * i; indexj < LargeurBandesV + LargeurBandesV * i; indexj++)
                                    {
                                        switch (CouleurDeLaBandeV)
                                        {
                                            case 0:
                                                Image[indexi, indexj].NuanceDeGris();
                                                break;

                                            case 1:
                                                Image[indexi, indexj].NuanceDeNoir();
                                                break;

                                            case 2:
                                                Image[indexi, indexj].NuanceDeBleu();
                                                break;

                                            case 3:
                                                Image[indexi, indexj].NuanceDeRouge();
                                                break;

                                            case 4:
                                                Image[indexi, indexj].NuanceDeVert();
                                                break;

                                            case 5:
                                                Image[indexi, indexj].NuanceDeJaune();
                                                break;

                                            case 6:
                                                Image[indexi, indexj].NuanceDeMagenta();
                                                break;

                                            case 7:
                                                Image[indexi, indexj].NuanceDeCyan();
                                                break;

                                            case 8:
                                                Image[indexi, indexj].NuanceDOrange();
                                                break;

                                            case 9:
                                                Image[indexi, indexj].NuanceDeRose();
                                                break;
                                        }
                                    }
                                }
                                
                            }
                            Image_To_File();
                            break;

                        #endregion

                        #region Horizontal

                        case 1:
                            Console.Clear();
                            Console.WriteLine("Combien de bandes voulez-vous ?"); int NbBandesH = int.Parse(Console.ReadLine());
                            int HauteurBandesH = Hauteur / NbBandesH;
                            for (int i = 0; i < NbBandesH; i++)
                            {
                                int CouleurDeLaBandeH = MenuCouleur(i + 1);
                                for (int indexi = HauteurBandesH * i; indexi < HauteurBandesH *(i+1); indexi++)
                                {
                                    for (int indexj = 0; indexj < Largeur; indexj++)
                                    {
                                        switch (CouleurDeLaBandeH)
                                        {
                                            case 0:
                                                Image[indexi, indexj].NuanceDeGris();
                                                break;

                                            case 1:
                                                Image[indexi, indexj].NuanceDeNoir();
                                                break;

                                            case 2:
                                                Image[indexi, indexj].NuanceDeBleu();
                                                break;

                                            case 3:
                                                Image[indexi, indexj].NuanceDeRouge();
                                                break;

                                            case 4:
                                                Image[indexi, indexj].NuanceDeVert();
                                                break;

                                            case 5:
                                                Image[indexi, indexj].NuanceDeJaune();
                                                break;

                                            case 6:
                                                Image[indexi, indexj].NuanceDeMagenta();
                                                break;

                                            case 7:
                                                Image[indexi, indexj].NuanceDeCyan();
                                                break;

                                            case 8:
                                                Image[indexi, indexj].NuanceDOrange();
                                                break;

                                            case 9:
                                                Image[indexi, indexj].NuanceDeRose();
                                                break;
                                        }
                                    }
                                }

                            }
                            Image_To_File();
                            break;

                            #endregion
                    }
                    break;

                #endregion
            }
        }

        public int MenuPersonnalisation()
        {
            int Ligne = 0;
            string[] Choix = {"-> Vertical |", "-> Horizontal --" };
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

        public int MenuCouleur(int NumeroCouleur)
        {
            int Ligne = 0;
            string[] Choix = { "-> Blanc", "-> Noir","-> Rouge","-> Bleu","-> Vert","-> Jaune","-> Magenta","-> Cyan","-> Orange","-> Rose" };
            ConsoleKeyInfo cki;
            SelectionCouleur(Ligne, Choix, NumeroCouleur);
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
                SelectionCouleur(Ligne, Choix, NumeroCouleur);
                cki = Console.ReadKey();
            }
            return Ligne;
        }

        public void Selection(int Ligne, string[] Choix)
        {
            Console.Clear();
            Console.WriteLine("Quel sera le sens des bandes ?\n");
            for (int i = 0; i < Choix.Length; i++)
            {
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

        public void SelectionCouleur(int Ligne, string[] Choix, int NumeroCouleur)
        {
            Console.Clear();
            Console.WriteLine("Quel sera la couleur de la bande n°"+NumeroCouleur+" ?\n");
            for (int i = 0; i < Choix.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            
                if (i == 0 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i == 2 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (i == 3 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (i == 4 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i == 5 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i == 6 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                if (i == 7 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i == 8 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (i == 9 && i == Ligne)
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(Choix[i]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("\nUne fois votre choix sélectionné appuyez sur 'Enter'.");
        }
    }
}

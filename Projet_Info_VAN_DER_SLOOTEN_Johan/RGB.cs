using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Info_VAN_DER_SLOOTEN_Johan
{
    class RGB
    {
        #region Attributs

        private byte bleu;
        private byte vert;
        private byte rouge;

        #endregion

        #region Propriétés

        public byte Bleu
        {
            get { return bleu; }
            set { bleu = value; }
        }

        public byte Vert
        {
            get { return vert; }
            set { vert = value; }
        }

        public byte Rouge
        {
            get { return rouge; }
            set { rouge = value; }
        }

        #endregion

        //Constructeur

        public RGB(byte[] couleur)
        {
            Rouge = couleur[0];
            Vert = couleur[1];
            Bleu = couleur[2];
        }


        /*-------------------------------------METHODES----------------------------------*/


        public void AfficherBytesCouleur()
        {
            if (Bleu < 10) Console.Write( Bleu + "   ");
            else if (Bleu < 100) Console.Write( Bleu + "  " );
            else Console.Write( Bleu + " ");

            if (Vert < 10) Console.Write(Vert + "   ");
            else if (Vert < 100) Console.Write(Vert + "  ");
            else Console.Write(Vert + " ");

            if (Rouge < 10) Console.Write(Rouge + "   ");
            else if (Rouge < 100) Console.Write(Rouge + "  ");
            else Console.Write(Rouge + " ");
        }

        #region Nuancier

        public void NuanceDeGris()
        {
            byte Gris = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Bleu = Gris;
            Vert = Gris;
            Rouge = Gris;
        }

        public void NuanceDeRouge()
        {
            Bleu = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Rouge = 0;            
            Vert = 0;
        }

        public void NuanceDeBleu()
        {
            Rouge = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Bleu = 0;            
            Vert = 0;
        }

        public void NuanceDeVert()
        {
            Vert = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Rouge = 0;
            Bleu = 0;
        }

        public void NuanceDeJaune()
        {
            byte Jaune = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Vert = Jaune;
            Rouge = 0;
            Bleu = Jaune;
        }

        public void NuanceDeMagenta()
        {
            Byte Magenta = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Vert = 0;
            Rouge = Magenta;
            Bleu = Magenta;
        }

        public void NuanceDeCyan()
        {
            Byte Cyan = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Vert = Cyan;
            Rouge = Cyan;
            Bleu = 0;
        }

        public void NuanceDOrange()
        {
            Byte Orange = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Vert = Convert.ToByte(Orange / 2);
            Rouge = 0;
            Bleu = Orange;
        }

        public void NuanceDeRose()
        {
            Byte Rose = Convert.ToByte((Bleu + Vert + Rouge) / 3);
            Vert = 0;
            Rouge = Convert.ToByte(Rose / 2);
            Bleu = Rose;
        }

        public void NuanceDeNoir()
        {
            byte Noir = Convert.ToByte((Bleu + Vert + Rouge) / 6);
            Bleu = Noir;
            Vert = Noir;
            Rouge = Noir;
        }

        public void NuanceDeBlanc()
        {
            byte Noir = Convert.ToByte(225-(Bleu + Vert + Rouge) / 6);
            Bleu = Noir;
            Vert = Noir;
            Rouge = Noir;
        }

        #endregion

        public void NoirEtBlanc()
        {
            if (Convert.ToByte((Bleu + Vert + Rouge) / 3) < 127) { Vert = 0; Bleu = 0; Rouge = 0; }
            else { Vert = 255; Bleu = 255; Rouge = 255; }
        }

        public void CloneCouleur(RGB ACloner)
        {            
            Rouge = ACloner.Rouge;
            Vert = ACloner.Vert;
            Bleu = ACloner.Bleu;
        }        
    }
}

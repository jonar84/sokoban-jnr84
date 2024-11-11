using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sokoban
{
    /// <summary>
    /// 
    /// </summary>
    public class Jeu
    {
        public enum Etat
        {
            Mur,
            Cible,
            Vide
        }

        public Etat[,] grille;

        public List<Position> caisse;

        public Position personnage;

        private int nbrDepla;

        static String grilleTxt = "..XXXXXX..XXX.oo.XXXX..o..o..XX........XXXX....XXX..XX.CXX...XXXC.XXX..X.CP.C.X..X......X..XXXXXXXX.";

        public int NbrDepla
        {
            get
            {
                return nbrDepla;
            }
        }

        public Jeu()
        {
            grille = new Etat[10, 10];
            initJeu();
            nbrDepla = 0;
        }

        private void initJeu()
        {
            caisse = new List<Position>();

            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    switch (grilleTxt[ligne * 10 + colonne])
                    {
                        case '.':
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'X':
                            grille[ligne, colonne] = Etat.Mur;
                            break;
                        case 'o':
                            grille[ligne, colonne] = Etat.Cible;
                            break;
                        case 'C':
                            caisse.Add(new Position(ligne, colonne));
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                        case 'P':
                            personnage = new Position(ligne, colonne);
                            grille[ligne, colonne] = Etat.Vide;
                            break;
                    }
                }
            }         
        }

        public void ToucheAppyer(Key key)
        {
            Position newPos = new Position(personnage.x, personnage.y);

            CalculNewPos(newPos, key);
            if (CaseOK(newPos, key))
            {
                personnage = newPos;
                nbrDepla++;
            }

        }

        public bool Fini()
        {
            foreach (Position caiss in caisse)
            {
                if (grille[caiss.x, caiss.y] != Etat.Cible)
                {
                    return false;
                }              
            }
            return true;
        }

        private static void CalculNewPos(Position newPos, Key key)
        {
            switch (key)
            {
                case Key.Down:
                    newPos.x++;
                    break;
                case Key.Up:
                    newPos.x--;
                    break;
                case Key.Right:
                    newPos.y++;
                    break;
                case Key.Left:
                    newPos.y--;
                    break;
            }
        }

        private bool CaseOK(Position newPos, Key key)
        {
            // Presence d'un mur
            if (grille[newPos.x, newPos.y] == Etat.Mur)
            {
                return false;
            }

            Position caisse = caisseInPos(newPos);

            if ( caisse != null)
            {
                // Deplacement possible de la caisse
                Position newPositionCaisse = new Position(caisse.x, caisse.y);
                CalculNewPos(newPositionCaisse, key);

                if (grille[newPositionCaisse.x, newPositionCaisse.y] == Etat.Mur)
                {
                    return false;
                }
                else if (caisseInPos(newPositionCaisse) != null)
                {
                    return false;
                }
                else
                {
                    caisse.x = newPositionCaisse.x;
                    caisse.y = newPositionCaisse.y;
                    return true;
                }

            }
            return true;


            // Presence d'une caisse sur la case ou le personnage va y aller
        }

        public void Restart()
        {
            initJeu();
            nbrDepla = 0;
        }

        private Position caisseInPos(Position newPos)
        {
            foreach (Position caissBois in caisse)
            {
                if (caissBois.x == newPos.x && caissBois.y == newPos.y)
                {
                    return caissBois;
                }
            }
            return null;
        }

        public Etat Case(int ligne, int colonne)
        {
            return grille[ligne, colonne];
        }
    }
}

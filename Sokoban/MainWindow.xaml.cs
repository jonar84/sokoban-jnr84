using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sokoban
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Jeu jeu;
        public MainWindow()
        {
            InitializeComponent();
            jeu = new Jeu();
            this.KeyDown += MainWindow_KeyDown;
            Dessiner();//DessinerCarte
            Redessiner();//Dessiner caisse et perso
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Right) || e.Key.Equals(Key.Left) || e.Key.Equals(Key.Up) || e.Key.Equals(Key.Down))
            {
                jeu.ToucheAppyer(e.Key);
                Redessiner();
            }
            if (jeu.Fini())
            {
                MessageBoxResult msg = MessageBox.Show("Congratulation! You win.");
            }
        }

        private void Redessiner()
        {
            cnvMobile.Children.Clear();
            DessinerCaisse();
            DessinerPerso();
            AfficherNbr();
        }

        private void AfficherNbr()
        {
            txtMoveNumber.Text = jeu.NbrDepla.ToString();
        }

        private void DessinerPerso()
        {
            Rectangle r = new Rectangle();
            r.Width = 50;
            r.Height = 50;
            r.Margin = new Thickness(jeu.personnage.y *50, jeu.personnage.x *50, 0, 0);
            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/character.png", UriKind.Relative)));
            cnvMobile.Children.Add(r);
        }

        private void DessinerCaisse()
        {
            foreach (Position pos in jeu.caisse)
            {
                Rectangle r = new Rectangle();
                r.Width = 42;
                r.Height = 42;
                r.Margin = new Thickness(pos.y *50+4, pos.x *50+4, 0, 0);
                r.Fill = new ImageBrush(new BitmapImage(new Uri("images/box.png", UriKind.Relative)));
                cnvMobile.Children.Add(r);
            }
        }

        private void Dessiner()
        {
            DessinerCarte();
        }

        private void DessinerCarte()
        {
            for (int ligne = 0; ligne < 10; ligne++)
            {
                for (int colonne = 0; colonne < 10; colonne++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = 50;
                    r.Height = 50;
                    r.Margin = new Thickness(colonne * 50, ligne * 50, 0, 0);

                    switch (jeu.Case(ligne, colonne))
                    {
                        case Jeu.Etat.Vide:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/vide.jpg", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Mur:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/mur.jpg", UriKind.Relative)));
                            break;
                        case Jeu.Etat.Cible:
                            r.Fill = new ImageBrush(new BitmapImage(new Uri("images/diamond_2.png", UriKind.Relative)));
                            break;
                    }
                    cnvGrille.Children.Add(r);
                }
            }
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            jeu.Restart();
            Redessiner();
        }
    }
}

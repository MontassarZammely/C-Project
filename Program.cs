
using System;
 
namespace checklist {
    class Program {
        
        // Méthode pour afficher le menu principal
        static void menu(){
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" _________________________________________________ ");
            Console.WriteLine("|                MENU D'INVENTAIRE                |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|           Tapez 'new' pour une nouvelle liste    |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|           Tapez 'view' pour afficher la liste    |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|   Tapez 'lists' pour voir toutes les listes      |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|   Tapez 'delete' pour supprimer une liste        |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|   Tapez 'quit' pour revenir au menu depuis une liste |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|              Tapez 'quit' pour quitter           |");
            Console.WriteLine("|                                                 |");
            Console.WriteLine("|     Si vous choisissez 'view' et que la liste    |");
            Console.WriteLine("|     n'existe pas, une liste vide sera créée.     |");
            Console.WriteLine("|_________________________________________________|");
            Console.ResetColor();
        }

        static void Main(string[] args){
            // Instanciation de la classe "inventory" et "files"
            inventory inv = new inventory();
            files file = new files();

            while(true){
                // Afficher le menu
                menu();
                Console.Write("> ");
                string input = Console.ReadLine();
                switch(input){
                    // Créer une nouvelle liste
                    case "new":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("Comment voulez-vous appeler la liste ? \n");
                        Console.Write("Nom de la liste : ");
                        input = Console.ReadLine();
                        inv.new_list(input);
                        break;
                    // Afficher une liste existante ou créer une liste vide si elle n'existe pas
                    case "view":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("Quelle liste voulez-vous voir ? \n");
                        Console.Write("Nom de la liste : ");
                        input = Console.ReadLine();
                        inv.view_list(input);
                        break;
                    // Afficher toutes les listes existantes
                    case "lists":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        file.show_all_lists();
                        Console.ResetColor();
                        break;
                    // Supprimer une liste existante
                    case "delete":
                        Console.Clear();
                        Console.WriteLine("\n");
                        Console.Write("Quelle liste voulez-vous supprimer ? \n");
                        Console.Write("Nom de la liste : ");
                        input = Console.ReadLine();
                        file.delete_list(input);
                        break;
                    // Quitter l'application
                    case "quit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Entrée inconnue, veuillez réessayer..");
                        break;
                }
            }
        }
    }
}


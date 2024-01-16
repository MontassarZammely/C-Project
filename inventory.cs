
using System; 
using System.Data.SQLite;

namespace checklist
{
    public class inventory {

        // Instanciation de la classe "files" pour gérer les fichiers
        files fileC = new files();

        // Méthode pour vérifier l'état non cochée
        public bool unchecked_status(string item, int id, string list){
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = "SELECT * FROM inventory";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                    using(SQLiteDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            if(reader["status"].Equals("[ ]") && reader["item"].Equals($"{item}")){
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        // Méthode pour vérifier l'état cochée
        public bool checked_status(string item, int id, string list){
            using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                connection.Open();
                string sql = "SELECT * FROM inventory";
                using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                    command.ExecuteNonQuery();
                    using(SQLiteDataReader reader = command.ExecuteReader()){
                        while(reader.Read()){
                            if(reader["status"].Equals("[X]") && reader["item"].Equals($"{item}")){
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        
        // Méthode pour créer une nouvelle liste
        public void new_list(string name){
            try {
                // Vérifier si la liste existe déjà
                if(fileC.check_for_list(name) == true){
                    Console.WriteLine($"la liste nommée {name} existe déjà.. veuillez utiliser un autre nom.");
                }
                else{ 
                    // Création du fichier SQLite pour la nouvelle liste
                    SQLiteConnection.CreateFile($"{name}.db");
                    using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {name}.db; Version = 3;")){
                        db_connection.Open();
                        // Création de la table "inventory" dans la base de données
                        string sql = "CREATE TABLE inventory (id INTEGER PRIMARY KEY AUTOINCREMENT, status VARCHAR(12), item VARCHAR(150), price REAL, date VARCHAR(50), description VARCHAR(255))";
                        using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.WriteLine("\n");
                    while(true){
                        Console.WriteLine("Entrez le nom de l'élément. Pour quitter, tapez 'quit' \n");
                        Console.Write("> ");
                        string item = Console.ReadLine();
                        if(item == "quit" || item == "Quit") {
                            break;
                        }
                        // Demander le prix de l'élément
                        Console.WriteLine("Entrez le prix de l'élément, ou pour sauter, tapez 'skip' \n");
                        Console.Write("> ");
                        float price = float.Parse(Console.ReadLine());
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sauté ! \n");
                            Console.ResetColor();
                            continue;
                        }
                        // Demander la date d'achat de l'élément
                        Console.WriteLine("Entrez la date d'achat de l'élément au format MM/JJ/AA, ou pour sauter, tapez 'skip' \n");
                        Console.Write("> ");
                        string date = Console.ReadLine();
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sauté ! \n");
                            Console.ResetColor();                            
                            continue;
                        }
                        // Demander une petite description pour l'élément
                        Console.WriteLine("Entrez une petite description pour l'élément, seulement 300 caractères sont autorisés dans ce champ. ou pour sauter, tapez 'skip' \n");
                        Console.Write("> ");
                        string desc = Console.ReadLine();
                        if(item == "skip" || item == "Skip") {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Sauté ! \n");
                            Console.ResetColor();
                            continue;
                        }
                        // Insérer l'élément dans la base de données
                        using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {name}.db; Version = 3;")){
                            db_connection.Open();
                            string sql = $"INSERT INTO inventory (status, item, price, date, description) VALUES ('[ ]', '{item}', '{price}', '{date}', '{desc}')"; 
                            using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Liste {name} créée. \n");
                    Console.ResetColor();
                }
            }
            catch(System.IO.IOException e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erreur {e} votre fichier de listes est en cours d'utilisation");
                Console.ResetColor();
            }
        }

        // Méthode pour afficher la liste
        public void view_list(string list){
            Console.Clear();
            while(true) {
                try {
                    // Connexion à la base de données
                    using(SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        db_connection.Open();
                        // Récupération de tous les éléments de la table "inventory"
                        string sql = "SELECT * FROM inventory";
                        using(SQLiteCommand command = new SQLiteCommand(sql, db_connection)){
                            command.ExecuteNonQuery();
                            using(SQLiteDataReader reader = command.ExecuteReader()){
                                Console.WriteLine("\nLe numéro derrière chaque '>' avec l'élément est la clé primaire de cet élément.");
                                Console.WriteLine("Certaines fonctions nécessitent la clé primaire pour fonctionner. Gardez cela à l'esprit.\n");
                                // Affichage de chaque élément avec sa description
                                while(reader.Read()){
                                    Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"]);
                                    Console.WriteLine("description : " + reader["description"] + "\n");
                                }
                            }
                        }
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("Appuyez sur Entrée pour continuer..");
                    Console.ReadLine();    
                    Console.WriteLine(" ___________________________________________________________________");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|        Tapez check si vous voulez cocher un élément de la liste.      |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|     Tapez uncheck si vous voulez décocher un élément de la liste.     |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|       Tapez remove si vous voulez supprimer un élément de la liste        |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|           Tapez add si vous voulez ajouter un élément à la liste            |");
                    Console.WriteLine("|                                                                   |");
                    Console.WriteLine("|              Tapez quit si vous voulez revenir au menu.               |");
                    Console.WriteLine("|___________________________________________________________________|");
                    Console.Write("> ");
                    string user_input = Console.ReadLine();
                    if(user_input == "check" || user_input == "Check"){
                        while(true){
                            checkinput:
                            Console.WriteLine("Quel élément voulez-vous cocher ? Tapez 'quit' pour quitter.");
                            Console.Write("> ");
                            string item = Console.ReadLine();
                            if(item == "quit" || item == "Quit"){
                                break;
                            }
                            if(item == "" || item == " "){
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Veuillez entrer un nom d'élément..");
                                Console.ResetColor();
                                goto checkinput; 
                            }
                            else{
                                Console.WriteLine($"Quelle est la clé primaire pour {item} ?");
                                Console.Write("> ");
                                int id = Convert.ToInt32(Console.ReadLine());
                                check(item, list, id);
                            }
                        }
                    }

                    // Autres fonctionnalités similaires pour uncheck, add, remove, etc.

                    else if(user_input == "quit" || user_input == "Quit") {
                        break;
                    }
                    
                    else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{user_input} n'est pas une commande valide, veuillez entrer une entrée valide..");
                        Console.ResetColor();
                    }
                }

                // Gestion des erreurs liées à la base de données SQLite
                catch(System.Data.SQLite.SQLiteException e){
                    SQLiteConnection db_connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;");
                    db_connection.Open();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERREUR {e} \n");
                    Console.ResetColor();
                    string sql = "CREATE TABLE inventory (id INTEGER PRIMARY KEY AUTOINCREMENT, status VARCHAR(12), item VARCHAR(150), price REAL, date VARCHAR(50), description VARCHAR(255))";
                    SQLiteCommand command = new SQLiteCommand(sql, db_connection);
                    command.ExecuteNonQuery();
                    db_connection.Close();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Erreur résolue");
                    Console.ResetColor();
                }
            }
        }
        
        // Méthode pour cocher un élément
        public void check(string item, string list, int id){
            try {
                if(checked_status(item, id, list) == true){
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{item} a déjà été coché.");
                    Console.ResetColor();
                }
                else {
                    // Mettre à jour l'état de l'élément à "[X]" dans la base de données
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        connection.Open();
                        string sql = $"UPDATE inventory SET status = '[X]' WHERE id = '{id}'";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                        }
                    }
                    Console.Clear();
                    // Afficher la liste mise à jour
                    using(SQLiteConnection connection = new SQLiteConnection($"Data Source = {list}.db; Version = 3;")){
                        connection.Open();
                        string sql = $"SELECT * FROM inventory";
                        using(SQLiteCommand command = new SQLiteCommand(sql, connection)){
                            command.ExecuteNonQuery();
                            using(SQLiteDataReader reader = command.ExecuteReader()){
                                while(reader.Read()){
                                    Console.WriteLine( reader["id"] + " > " + " " + reader["status"] + " " + reader["item"] + " " + reader["price"] + " " + reader["date"] + "\n");
                                }
                            }
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{item} a été coché ! \n");
                    Console.ResetColor();
                }
            }
            catch(Exception e){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERREUR {e} \n");
                Console.ResetColor();
            }
        }

        // Autres méthodes similaires pour uncheck, add_item, remove_item, etc.
    } 
}


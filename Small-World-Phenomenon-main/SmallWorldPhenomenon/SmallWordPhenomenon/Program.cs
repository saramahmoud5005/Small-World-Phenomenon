using System;
using System.IO;
using System.Collections.Generic;

namespace SmallWordPhenomenon
{
    public class links_num
    {
        public int value;
        public links_num()
        {
            this.value = int.MaxValue;
        }
    }
    public class links_cost
    {
        public int value;
        public links_cost()
        {
            this.value = 0;
        }
    }
    public class Pair
    {
        public string single_movie;
        public int movies_count;
        public Pair()
        {
            this.single_movie = "";
            this.movies_count = 0;
        }
        public Pair(string single_movie, int movies_count)
        {
            this.single_movie = single_movie;
            this.movies_count = movies_count;
        }
    }

    class Program
    {
        public static string project_path = @"C:\Users\AZ\OneDrive\Documents\GitHub\Small-Word-Phenomenon\SmallWordPhenomenon\SmallWordPhenomenon\";
        public static bool Checktestcase(string standard_sol, string our_sol)
        {
            int wrongans = 0, num_of_lines = 0;
            string expected = " ";
            string received = " ";
            bool succeeded = true;
            string standard_sol_file = project_path + standard_sol;
            string text = File.ReadAllText(standard_sol_file);
            string[] lines1 = text.Split("\n");

            string our_sol_file = project_path + our_sol;
            string text2 = File.ReadAllText(our_sol_file);
            string[] lines2 = text2.Split("\n");

            for (int i = 0; i < lines1.Length; ++i)
            {
                if (lines1[i] != lines2[i])
                {
                    expected = lines1[i];
                    received = lines2[i];
                    if (expected.IndexOf("DoS") == 0)
                    {
                        if (i % 5 == 0)
                            Console.WriteLine("Num of test case --> " + i / 5);
                        succeeded = false;
                        wrongans++;
                        Console.WriteLine("EXPECTED ANSWER: " + expected + "\n" + "RECIEVED: " + received);
                    }
                }
                num_of_lines++;
            }

            if (succeeded == true) Console.WriteLine("CONGRATULATIONS");

            else
            {
                Console.WriteLine("NO. OF WRONG CASES = " + wrongans + " FROM = " + num_of_lines / 5);
                Console.WriteLine("THERE IS SOMETHING WRONG");
            }

            //CLEARING OURSOL FILE
            File.WriteAllText(our_sol_file, String.Empty);
            return succeeded;
        }
        public static void WriteInSolFile(string our_sol, string node_a, string node_b, int DoS, int RS, List<string> best_path, Dictionary<string, Dictionary<string, Pair>> graph)
        {
            string path = project_path + our_sol;
            string[] solution = new string[7];
            solution[0] = node_a + "/" + node_b + "\n";
            solution[1] = "DoS = " + DoS + ", RS = " + RS + "\n";
            List<string> chain_of_films = new List<string>();
            List<string> chain_of_actors = new List<string>();

            for (int i = 0; i < best_path.Count; i++)
            {
                chain_of_actors.Add(best_path[i]);
                if (i < best_path.Count - 1)
                    chain_of_films.Add(graph[best_path[i]][best_path[i + 1]].single_movie);
            }

            solution[2] = "CHAIN OF ACTORS: ";
            for (int i = 0; i < chain_of_actors.Count; i++)
            {
                solution[2] += chain_of_actors[i];
                if (i != chain_of_actors.Count - 1)
                {
                    solution[2] += " -> ";
                }
            }
            solution[3] = "\n";
            solution[4] = "CHAIN OF MOVIES:  => ";
            for (int i = chain_of_films.Count - 1; i >= 0; i--)
            {
                solution[4] += chain_of_films[i];
                solution[4] += " =>";
                if (i - 1 != -1)
                {
                    solution[4] += " ";
                }
            }
            solution[5] = "\n" + "\n";
            StreamWriter sw = new StreamWriter(path, true);
            for (int i = 0; i < solution.Length; ++i)
            {
                if (solution[i] != "\n")
                    Console.WriteLine(solution[i]);
                sw.Write(solution[i]);
            }
            sw.Flush();
            sw.Close();
        }
        public static void ShortestPath
            (string node_a, Dictionary<string, links_num> F_DoS, Dictionary<string, links_cost> F_RS, Dictionary<string, string> F_pre_node, HashSet<string> F_Process, 
            string node_b, Dictionary<string, links_num> B_DoS, Dictionary<string, links_cost> B_RS, Dictionary<string, string> B_pre_node, HashSet<string> B_Process,
            Dictionary<string, Dictionary<string, Pair>> graph)
        {
            int DoS = int.MaxValue;
            int RS = 0;
            string best_node = "";
            //BEST DoS
            foreach (string node in F_Process) // o(v)
            {
                if(F_DoS.ContainsKey(node) && B_DoS.ContainsKey(node))
                {
                    if (F_DoS[node].value + B_DoS[node].value < DoS)
                    {
                        best_node = node;
                        DoS = F_DoS[node].value + B_DoS[node].value;
                        RS = F_RS[node].value + B_RS[node].value;
                    }
                    else if(F_DoS[node].value + B_DoS[node].value == DoS && F_RS[node].value + B_RS[node].value >= RS)
                    {
                        best_node = node;
                        DoS = F_DoS[node].value + B_DoS[node].value;
                        RS = F_RS[node].value + B_RS[node].value;
                    }
                }
            }
            foreach (string node in B_Process)
            {
                if (F_DoS.ContainsKey(node) && B_DoS.ContainsKey(node))
                {
                    if (F_DoS[node].value + B_DoS[node].value < DoS)
                    {
                        best_node = node;
                        DoS = F_DoS[node].value + B_DoS[node].value;
                        RS = F_RS[node].value + B_RS[node].value;
                    }
                    else if (F_DoS[node].value + B_DoS[node].value == DoS && F_RS[node].value + B_RS[node].value >= RS)
                    {
                        best_node = node;
                        DoS = F_DoS[node].value + B_DoS[node].value;
                        RS = F_RS[node].value + B_RS[node].value;
                    }
                }
            }
            //BEST PATH
            List<string> best_path = new List<string>();
            string last_node = best_node;
            while(last_node != node_a)
            {
                best_path.Add(last_node);
                last_node = F_pre_node[last_node];
            }
            best_path.Add(last_node);
            best_path.Reverse();
            last_node = best_node;
            while (last_node != node_b)
            {
                last_node = B_pre_node[last_node];
                best_path.Add(last_node);
            }
            WriteInSolFile("oursol.txt", node_a, node_b, DoS, RS, best_path, graph);
        }
        public static void ReleaseNodes(string parent_node, string child_node_name, Pair movies, ref Dictionary<string, links_num> DoS,
            ref Dictionary<string, links_cost> RS, ref Dictionary<string, string> pre_node, ref Priority_Queue next_to_visit)
        {
            if (!DoS.ContainsKey(child_node_name)) //o(1)
                DoS.Add(child_node_name, new links_num()); //O(1)
            if (!RS.ContainsKey(child_node_name))
                RS.Add(child_node_name, new links_cost());

            if (DoS[parent_node].value + 1 < DoS[child_node_name].value)
            {
                DoS[child_node_name].value = DoS[parent_node].value + 1;
                if (!pre_node.ContainsKey(child_node_name))
                    pre_node.Add(child_node_name, parent_node);
                pre_node[child_node_name] = parent_node;
                RS[child_node_name].value = RS[parent_node].value + movies.movies_count;
                next_to_visit.push(new triple(DoS[child_node_name].value, RS[child_node_name].value, child_node_name)); //o(n)
            }
            else if (DoS[parent_node].value + 1 == DoS[child_node_name].value)
            {
                if (RS[parent_node].value + movies.movies_count >= RS[child_node_name].value)
                {
                    if (!pre_node.ContainsKey(child_node_name))
                        pre_node.Add(child_node_name, parent_node);
                    pre_node[child_node_name] = parent_node;
                    RS[child_node_name].value = RS[parent_node].value + movies.movies_count;
                    next_to_visit.push(new triple(DoS[child_node_name].value, RS[child_node_name].value, child_node_name)); //o(n)
                }
            }
        }
        public static void BidirectionalDijkstra(string node_a, string node_b, Dictionary<string, Dictionary<string,Pair>> graph) //O(V^3)
        {
            Dictionary<string, links_num> F_DoS = new Dictionary<string, links_num>();
            Dictionary<string, links_num> B_DoS = new Dictionary<string, links_num>();
            Dictionary<string, links_cost> F_RS = new Dictionary<string, links_cost>();
            Dictionary<string, links_cost> B_RS = new Dictionary<string, links_cost>();
            Dictionary<string, string> F_pre_node = new Dictionary<string, string>();
            Dictionary<string, string> B_pre_node = new Dictionary<string, string>();
            Priority_Queue F_next_to_visit = new Priority_Queue();
            Priority_Queue B_next_to_visit = new Priority_Queue();
            HashSet<string> F_Process = new HashSet<string>();
            HashSet<string> B_Process = new HashSet<string>();

            //Forward path
            F_DoS.Add(node_a, new links_num()); // o(1)
            F_DoS[node_a].value = 0;
            F_pre_node.Add(node_a, node_a); // o(1)
            F_next_to_visit.push(new triple(0, 0, node_a));
            //Backward path
            B_DoS.Add(node_b, new links_num()); // o(1)
            B_DoS[node_b].value = 0;
            B_pre_node.Add(node_b, node_b); // o(1)
            B_next_to_visit.push(new triple(0, 0, node_b)); // o(n)

            while (true) // o(v)
            {
                string F_parent_node = F_next_to_visit.top().third; // o(1)
                string B_parent_node = B_next_to_visit.top().third; // o(1)
                F_next_to_visit.pop();
                B_next_to_visit.pop();
                F_Process.Add(F_parent_node);
                B_Process.Add(B_parent_node);

                //FORWARD
                if (!F_DoS.ContainsKey(F_parent_node))
                    F_DoS.Add(F_parent_node, new links_num());
                if (!F_RS.ContainsKey(F_parent_node))
                    F_RS.Add(F_parent_node, new links_cost());

                foreach (var (child_node_name, films) in graph[F_parent_node]) //O(V)
                    ReleaseNodes(F_parent_node, child_node_name, films, ref F_DoS, ref F_RS, ref F_pre_node, ref F_next_to_visit); //O(n)  
                
                F_Process.Add(F_parent_node);
                if(B_Process.Contains(F_parent_node))
                {
                    //FOUND SHORTEST PATH
                    ShortestPath(node_a, F_DoS, F_RS, F_pre_node, F_Process, node_b, B_DoS, B_RS, B_pre_node, B_Process, graph); // o(v)
                    return;
                }

                //BACKWARD
                if (!B_DoS.ContainsKey(B_parent_node))
                    B_DoS.Add(B_parent_node, new links_num());
                if (!B_RS.ContainsKey(B_parent_node))
                    B_RS.Add(B_parent_node, new links_cost());

                foreach (var (child_node_name, films) in graph[B_parent_node])
                    ReleaseNodes(B_parent_node, child_node_name, films, ref B_DoS, ref B_RS, ref B_pre_node, ref B_next_to_visit); //O(n)

                B_Process.Add(B_parent_node);
                if(F_Process.Contains(B_parent_node))
                {
                    //FOUND SHORTEST PATH
                    ShortestPath(node_a, F_DoS, F_RS, F_pre_node, F_Process, node_b, B_DoS, B_RS, B_pre_node, B_Process, graph); // o(v)
                    return;
                }

            }
        }
        public static string CreateGraph(string input_movies, string queries) // o(n^3)
        {
            Dictionary<string, Dictionary<string, Pair>> graph = new Dictionary<string, Dictionary<string, Pair>>();
            string input_movies_file = project_path + input_movies;
            string text = File.ReadAllText(input_movies_file);
            string[] movies_lines = text.Split("\n");
            string input_queries_file = project_path + queries;
            string text2 = File.ReadAllText(input_queries_file);
            string[] queries_lines = text2.Split("\n");
            //MOVIES
            for (int i = 0; i < movies_lines.Length; i++)//ITERATE ON LINES
            {
                char del = '/';
                String[] actors = movies_lines[i].Split(del);
                string movie_name = actors[0];
                for (int j = 1; j < actors.Length; j++)//ITERATE ON ACTORS
                {
                    string parent_node = actors[j];

                    for (int k = j + 1; k < actors.Length; k++)
                    {
                        string child_node = actors[k];

                        //PARENT TO CHILD
                        if (!graph.ContainsKey(parent_node))
                        {
                            graph.Add(parent_node, new Dictionary<string, Pair>() { [child_node] = new Pair(movie_name, 1) });
                        }
                        else if (!graph[parent_node].ContainsKey(child_node))
                        {
                            graph[parent_node].Add(child_node, new Pair(movie_name, 1));
                        }
                        else
                        {
                            graph[parent_node][child_node].movies_count++;
                        }

                        //CHILD TO PARENT
                        if (!graph.ContainsKey(child_node))
                        {
                            graph.Add(child_node, new Dictionary<string, Pair>() { [parent_node] = new Pair(movie_name, 1) });
                        }
                        else if (!graph[child_node].ContainsKey(parent_node))
                        {
                            graph[child_node].Add(parent_node, new Pair(movie_name, 1));
                        }
                        else
                            graph[child_node][parent_node].movies_count++;
                    }

                }

            }
            //QUERIES
            for (int i = 0; i < queries_lines.Length - 1; i++)
            {
                char del = '/';
                String[] query = queries_lines[i].Split(del);
                string node_a = query[0];
                string node_b = query[1];
                BidirectionalDijkstra(node_a, node_b, graph);
            }
            return "EXIT_SUCCESS";
        }
        static void Main(string[] args)
        {
            char choice;
            char ans;
            int numchoice;

            Console.WriteLine("Small world pheonmoen problem :\n");
            Console.WriteLine("Do you want to run complete Testing now (y/n) ?");
            choice = Console.ReadLine()[0];
            if (choice == 'n' || choice == 'N') { return; }
            else if (choice == 'y' || choice == 'Y')
            {
                Console.WriteLine("[1]small \n[2]medium \n[3]large\n[4]extreme  ");
                numchoice = Convert.ToInt32(Console.ReadLine());

                switch (numchoice)
                {
                    case 1:
                        CreateGraph("Complete/small/Case1/Movies193.txt", "Complete/small/Case1/queries110.txt");
                        Checktestcase("Complete/small/Case1/Solution/queries110 - Solution.txt", "oursol.txt");
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/small/Case2/Movies187.txt", "Complete/small/Case2/queries50.txt");
                            Checktestcase("Complete/small/Case2/Solution/queries50 - Solution.txt", "oursol.txt");
                        }
                        break;
                    case 2:
                        CreateGraph("Complete/medium/Case1/Movies967.txt", "Complete/medium/Case1/queries85.txt");
                        Checktestcase("Complete/medium/Case1/Solutions/queries85 - Solution.txt", "oursol.txt");
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/medium/Case1/Movies967.txt", "Complete/medium/Case1/queries4000.txt");
                            Checktestcase("Complete/medium/Case1/Solutions/queries4000 - Solution.txt", "oursol.txt");
                        }
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/medium/Case2/Movies4736.txt", "Complete/medium/Case2/queries110.txt");
                            Checktestcase("Complete/medium/Case2/Solutions/queries110 - Solution.txt", "oursol.txt");
                        }
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/medium/Case2/Movies4736.txt", "Complete/medium/Case2/queries2000.txt");
                            Checktestcase("Complete/medium/Case2/Solutions/queries2000 - Solution.txt", "oursol.txt");
                        }
                        break;
                    case 3:
                        CreateGraph("Complete/large/Movies14129.txt", "Complete/large/queries26.txt");
                        Checktestcase("Complete/large/Solutions/queries26 - Solution.txt", "oursol.txt");
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/large/Movies14129.txt", "Complete/large/queries600.txt");
                            Checktestcase("Complete/large/Solutions/queries600 - Solution.txt", "oursol.txt");
                        }
                        break;
                    case 4:
                        CreateGraph("Complete/extreme/Movies122806.txt", "Complete/extreme/queries22.txt");
                        Checktestcase("Complete/extreme/Solutions/queries22 - Solution.txt", "oursol.txt");
                        Console.WriteLine("===================================================================================");
                        Console.WriteLine("Do you want to test another cases (y/n)?");
                        ans = Console.ReadLine()[0];
                        if (choice == 'n' || choice == 'N') { return; }
                        else if (choice == 'y' || choice == 'Y')
                        {
                            CreateGraph("Complete/extreme/Movies122806.txt", "Complete/extreme/queries200.txt");
                            Checktestcase("Complete/extreme/Solutions/queries200 - Solution.txt", "oursol.txt");
                        }
                        break;
                }
            }
        }
    }
}
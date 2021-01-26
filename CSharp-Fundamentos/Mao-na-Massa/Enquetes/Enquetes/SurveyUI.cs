using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enquetes
{
    /// <summary>
    /// Gerencia a interface gráfica da aplicação.
    /// </summary>
    class SurveyUI
    {
        /// <summary>
        /// Enquete ativa.
        /// </summary>
        private Survey survey;

        /// <summary>
        /// Arquivo associado à enquete.
        /// </summary>
        private string surveyFile;


        /// <summary>
        /// Inicia a execução da aplicação.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                // Mostra o menu principal. O retorno é a opção escolhida.
                string option = ShowMainMenu();

                switch (option)
                {
                    case "1":
                        // Cria uma enquete e mostra o menu de enquete.
                        ShowCreateMenu();
                        ShowSurveyMenu();
                        break;

                    case "2":
                        // Carrega uma enquete e mostra o menu de enquete.
                        ShowLoadMenu();
                        ShowSurveyMenu();
                        break;

                    default:
                        // Sair da aplicação.
                        return;
                }
            }
        }


        /// <summary>
        /// Mostra o menu principal.
        /// </summary>
        /// <returns>Opção escolhida.</returns>
        private string ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("MENU PRINCIPAL");
                Console.WriteLine("--------------");

                Console.WriteLine("1 - Criar uma enquete");
                Console.WriteLine("2 - Carregar uma enquete");
                Console.WriteLine("3 - Sair");

                WriteWhiteLines(1);

                Console.Write("O que você deseja fazer? => ");

                string option = Console.ReadLine();

                if (option != "1" && option != "2" && option != "3")
                {
                    // Enquanto a opção digitada for inválida, fica no loop.
                    continue;
                }

                return option;
            }
        }


        /// <summary>
        /// Mostra o menu de criar enquete.
        /// </summary>
        private void ShowCreateMenu()
        {
            survey = new Survey();
            surveyFile = null;

            Console.Clear();

            Console.WriteLine("CRIAR UMA NOVA ENQUETE");
            Console.WriteLine("----------------------");

            WriteWhiteLines(1);

            while (true)
            {
                // Solicita a pergunta da enquete.
                Console.Write("Pergunta: ");
                string question = Console.ReadLine();
                if (!string.IsNullOrEmpty(question))
                {
                    survey.Question = question;
                    break;
                }
            }

            WriteWhiteLines(1);

            int numOptions;
            while (true)
            {
                // Solicita o número de opções.
                Console.Write("Quantas opções a pergunta vai ter? ");

                if (int.TryParse(Console.ReadLine(), out numOptions))
                {
                    break;
                }
            }

            // Espaçamento entre as etapas
            WriteWhiteLines(2);

            // Solicita cada uma das opções (ID e texto)
            for (int i = 0; i < numOptions; i++)
            {
                string id;
                string text;

                while (true)
                {
                    Console.Write("ID da opção {0}: ", i + 1);
                    id = Console.ReadLine();
                    if (!string.IsNullOrEmpty(id))
                    {
                        break;
                    }
                }

                while (true)
                {
                    Console.Write("Texto da opção {0}: ", i + 1);
                    text = Console.ReadLine();
                    if (!string.IsNullOrEmpty(text))
                    {
                        break;
                    }
                }

                survey.SetOption(id, text);

                WriteWhiteLines(1);
            }

            WriteWhiteLines(2);

            // Mostra a enquete.
            Console.WriteLine("Opções adicionadas com sucesso! Veja a enquete:\n");
            Console.WriteLine(survey.GetFormattedSurvey());

            WriteWhiteLines(2);

            while (true)
            {
                // Solicita um arquivo para gravação da nova enquete.
                Console.Write("Digite o caminho do arquivo para salvar a enquete: ");
                string filePath = Console.ReadLine();

                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        // Salva a enquete no arquivo.
                        SurveyIO.SaveToFile(survey, filePath);
                        surveyFile = filePath;
                        break;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Ocorreu um erro ao salvar o arquivo: " + ex.Message);
                    }
                }
            }

            WriteWhiteLines(1);

            Console.WriteLine("Enquete salva em \"{0}\". Pressione ENTER para continuar...", surveyFile);
            Console.ReadLine();
        }
        
        /// <summary>
        /// Mostra o menu de carregamento de enquete.
        /// </summary>
        private void ShowLoadMenu()
        {
            survey = new Survey();

            Console.Clear();

            Console.WriteLine("CRIAR UMA NOVA ENQUETE");
            Console.WriteLine("----------------------");

            WriteWhiteLines(1);

            while (true)
            {
                // Solicita o caminho onde a enquete está gravada.
                Console.Write("Digite o nome do arquivo da enquete: ");
                string filePath = Console.ReadLine();

                if (!string.IsNullOrEmpty(filePath))
                {
                    try
                    {
                        // Carrega a enquete do arquivo.
                        SurveyIO.LoadFromFile(survey, filePath);
                        surveyFile = filePath;

                        Console.WriteLine("A enquete foi carregada com sucesso! Pressione ENTER para continuar...");
                        Console.ReadLine();
                        break;
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Ocorreu um erro ao abrir o arquivo: " + ex.Message);
                    }
                }
            }
        }


        /// <summary>
        /// Mostra o menu de enquete, onde é possível votar ou ver o resultado.
        /// </summary>
        private void ShowSurveyMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("MENU DE ENQUETE");
                Console.WriteLine("---------------");

                WriteWhiteLines(2);

                Console.WriteLine("Enquete ativa: \"{0}\"", survey.Question);
                Console.WriteLine("Número de votos: {0}", survey.VoteCount);

                WriteWhiteLines(1);

                Console.WriteLine("1 - Votar na enquete");
                Console.WriteLine("2 - Ver resultado da enquete");
                Console.WriteLine("3 - Voltar ao menu principal");

                WriteWhiteLines(1);

                Console.Write("Escolha uma opção => ");
                string option = Console.ReadLine();

                if (option == "1")
                {
                    // Vota na enquete.
                    ShowVoteMenu();
                }
                else if (option == "2")
                {
                    // Mostra resultado da enquete.
                    ShowSurveyResults();
                }
                else if (option == "3")
                {
                    // Volta para o menu principal.
                    break;
                }
            }
        }


        /// <summary>
        /// Mostra o menu de votação da enquete.
        /// </summary>
        private void ShowVoteMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("VOTAR");
                Console.WriteLine("-----");

                WriteWhiteLines(1);

                Console.WriteLine("Quantidade de votos: {0}", survey.VoteCount);

                WriteWhiteLines(1);

                Console.WriteLine(survey.GetFormattedSurvey());
                Console.Write("Escolha uma opção => ");

                Option option;
                string vote;

                bool isValid = survey.Vote(out option, out vote);

                if (isValid)
                {
                    WriteWhiteLines(1);

                    Console.Write("Obrigado pelo seu voto! Deseja continuar votando? (S/N): ");

                    string yesNo = Console.ReadLine();

                    if (yesNo != "S" && yesNo != "s")
                    {
                        break;
                    }
                }
            }

            WriteWhiteLines(1);

            // Ao final da votação, salva a enquete no arquivo associado.
            SurveyIO.SaveToFile(survey, surveyFile);

            Console.Write("Fim da votação. Pressione ENTER para continuar...");
            Console.ReadLine();
        }

        /// <summary>
        /// Mostra o resultado da enquete.
        /// </summary>
        private void ShowSurveyResults()
        {
            Console.Clear();

            Console.WriteLine("RESULTADO DA ENQUETE");
            Console.WriteLine("--------------------");

            WriteWhiteLines(1);

            // Calcula o resultado.
            var scores = survey.CalculateScores();

            Console.WriteLine("{0,-23} | {1,-5}", "Opção", "Votos");
            Console.WriteLine("-------------------------------");

            foreach (var score in scores)
            {
                Console.WriteLine("{0,-3}{1,-20} | {2,5}", score.Option.Id, score.Option.Text, score.Count);
            }

            WriteWhiteLines(1);

            Console.WriteLine("Pressione ENTER para continuar...");
            Console.ReadLine();
        }


        /// <summary>
        /// Escreve no console linhas em branco (espaços).
        /// </summary>
        /// <param name="numberOfLines">Quantidade de linhas/espaços desejados.</param>
        private void WriteWhiteLines(int numberOfLines)
        {
            for (int i = 0; i < numberOfLines; i++)
                Console.WriteLine();
        }
    }
}

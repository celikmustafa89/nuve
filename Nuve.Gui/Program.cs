﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Nuve.Lang;
using Nuve.Morphologic.Structure;
using Nuve.NGrams;
using Nuve.Sentence;
using Nuve.Stemming;

namespace Nuve.Gui
{
    internal static class Program
    {
        
        private const string TaggedInput = @"C:\Users\hrzafer\Dropbox\nuve\corpus\tcSentencedNormalized.txt";
        private const string UntaggedInput = @"C:\Users\hrzafer\Dropbox\nuve\corpus\tcNormalized.txt";

        private static readonly WordAnalyzer Analyzer = null;
        //private static readonly WordAnalyzer Analyzer = new WordAnalyzer(Language.Turkish);

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
           
            //var deserializedProduct = JsonConvert.DeserializeObject<NGramDictionary>(output);

            ////var model = CreateModel();

            //var model = new NGramModel(2, @"C:\Users\hrzafer\Desktop\workspace\Prizma\code\prizma\src\main\resources\stemDict\model_uni_bi.json");

            //IStemmer stemmer = new DictionaryStemmer();
            ////var betterModel = CreateBetterModel(stemmer);
            //var betterModel = new NGramModel(2, @"C:\Users\hrzafer\Desktop\workspace\Prizma\code\prizma\src\main\resources\stemDict\model_uni_bi.json");

            //IStemmer betterStemmer = new StatisticalStemmer(betterModel, Analyzer);

            ////Console.WriteLine(stemmer.GetStem("araştırmalardı"));
            ////Console.WriteLine(stemmer.GetStem("annesiydi"));

            //StemmerEvaluator.Evaluate(stemmer, @"C:\Users\hrzafer\Dropbox\nuve\data\expected_stems.txt");

            //StemmerEvaluator.Evaluate(betterStemmer, @"C:\Users\hrzafer\Dropbox\nuve\data\expected_stems.txt");

            //Test();

            //var words = File.ReadAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\suggestion\unigrams.txt")
            //    .Select(x => x.Split(null)[0]);
            //var output = @"C:\Users\hrzafer\Desktop\workspace\Prizma\code\prizma\src\main\resources\stemDict\nuve_stems2.dict";

            //StemDictionaryGenerator.Generate(words, betterStemmer, output);

            //StemFirst500();

            //GetProbabilities("kitaplar");

            //var extractor = new NGramExtractor(NGramSize.Trigram);
            //var tokens = File.ReadAllText(@"C:\Users\hrzafer\Dropbox\nuve\corpus\tcNormalized.txt").Split(null).ToList();
            //tokens.RemoveAll(x => x.Length == 0);
            //var map = extractor.ExtractAsDictionary(tokens);
            //ToSortedFile(map, @"C:\Users\hrzafer\Dropbox\nuve\corpus\bigrams.txt");
        }

        public static void GetProbabilities(string word)
        {
            var model = CreateModel();

            var solutions = Analyzer.Analyze(word);
            foreach (var solution in solutions)
            {
                //var ids = VARIABLE.GetMorphemeIds();
                var p1 = model.GetSentenceProbability(solution.GetMorphemeIds());
                Console.WriteLine(solution);
                Console.WriteLine("p1:{0}", p1);
            }
        }


        public static void ToSortedFile(IDictionary<string, int> map, string path)
        {
            var list = map.ToList();
            list.Sort((first, next) => next.Value.CompareTo(first.Value));
            var text = list.Select(x => x.Key + "\t" + x.Value);
            File.WriteAllLines(path, text);
        }

        public static void EvaluateSbd(SentenceSegmenter segmenter)
        {
            string[] taggedParagraphs = File.ReadAllLines(TaggedInput);
            IEnumerable<DetailedEvaluation> evaluations = segmenter.Evaluate(taggedParagraphs);
            SentenceSegmenterEvaluator.GetTotalReport(evaluations, printFalseAlarms: true);
        }

        public static void PrintSentences(SentenceSegmenter segmenter, IEnumerable<string> paragraphs)
        {
            foreach (string paragraph in paragraphs)
            {
                PrintSentences(segmenter, paragraph);
            }
        }

        public static void PrintSentences(SentenceSegmenter segmenter, string paragraph)
        {
            IEnumerable<string> sentences = segmenter.GetSentences(paragraph);      
            foreach (string sentence in sentences)
            {
                Console.WriteLine(sentence);
            }
        }


        public static void Test()
        {
            string[] testStrings =
            {
                "görüşemeyeli", "özgeçmiş", "özgeçmişin", "halükarda", "yapmadık", "araştırmalardı", "kalemlerin",
                "kalemlerden"   
            };
            //string[] testStrings = SoruTest.Soru;
            try
            {
                AnalysisHelper.Analyze(Analyzer, testStrings);

                //string test = TestGenerator.GenerateContainsAnalysisTest(SpecialCase.Şapkalı, "Şapkalı");
                //string test = TestGenerator.GenerateContainsAnalysesTest(VerbAux.FiilimsiZarfArak, "FiilimsiZarfArak", "FIILIMSI_ZARF_(y)ArAk");
                //Console.WriteLine(test);

                //Benchmarker.TestWithAMillionTokens(analyzer);
                //Benchmarker.TestWithAMillionWords(analyzer);
                //Benchmarker.TestMillionTimesWithSingleWord("kitabımdakin", analyzer);    
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.InnerException.ToString());
            }
        }

        private static void StemFirst500()
        {
            var words =
                File.ReadAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\suggestion\unigrams.txt")
                    //.Select(x => x.Split(null)[0])
                    .Where(x => x.StartsWith("kale"))
                    .ToArray();
            //var lines = new List<string>();


            //for (int i = 0; i < 1500; i++)
            //{
            //    var solutions = Analyzer.Analyze(words[i]);
            //    if (solutions.Count > 1)
            //    {
            //        var sb = new StringBuilder(words[i]).Append("\n");

            //        foreach (var sol in solutions)
            //        {
            //            sb.Append("\t").Append(sol.GetStem()).Append("\n");
            //        }
            //        lines.Add(sb.ToString());
            //    }
            //}

            File.WriteAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\suggestion\kalem_stems.txt", words);
        }

        private static NGramModel CreateBetterModel(IStemmer stemmer)
        {
            var lines = File.ReadAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\suggestion\unigrams.txt")
                .Select(x => x.Split(null));
            var nGramModel = new NGramModel(2);

            int counter = 0;

            foreach (var line in lines)
            {
                counter++;
                var solutions = Analyzer.Analyze(line[0]);
                foreach (var solution in solutions)
                {
                    if (solutions.Count==1 || stemmer.GetStem(line[0]) == solution.GetStem().GetSurface())
                    {
                        var morphemeIds = solution.GetMorphemeIds();
                        var times = Math.Round((Int32.Parse(line[1]) + 99) / (double)100);
                        for (int i = 0; i < times; i++)
                        {
                            nGramModel.AddSentence(morphemeIds);
                        }
                    }                    
                }

                if (counter % 100 == 0)
                {
                    Console.WriteLine(counter);
                }
            }

            nGramModel.Deserialize(@"C:\Users\hrzafer\Desktop\workspace\Prizma\code\prizma\src\main\resources\stemDict\model_uni_bi.json");

            return nGramModel;
        }

        private static NGramModel CreateModel()
        {
            var lines = File.ReadAllLines(@"C:\Users\hrzafer\Desktop\workspace\Damla\code\suggestion\unigrams.txt")
                .Select(x => x.Split(null));
            var nGramModel = new NGramModel(2);

            int counter = 0;

            foreach (var line in lines)
            {
                counter++;
                var solutions = Analyzer.Analyze(line[0]);
                foreach (var solution in solutions)
                {
                    var morphemeIds = solution.GetMorphemeIds();
                    var times = Math.Round((Int32.Parse(line[1]) + 99)/(double)100);
                    for (int i = 0; i < times; i++)
                    {
                        nGramModel.AddSentence(morphemeIds);
                    }
                }

                if (counter%100 == 0)
                {
                    Console.WriteLine(counter);
                }
            }

            nGramModel.Deserialize(@"C:\Users\hrzafer\Desktop\workspace\Prizma\code\prizma\src\main\resources\stemDict\model_uni_bi.json");

            return nGramModel;
        }

        private static IList<string> ReadWords(string filename)
        {
            string[] tokens = File.ReadAllText(filename, Encoding.UTF8).Split(null);
            return tokens.ToList();
        }
    }
}

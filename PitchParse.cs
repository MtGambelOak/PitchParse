/*
    PitchParse is a relatively simple console application that utilizes available pitch accent and frequency data to streamline the process of learning the pitch accent for words.
    It also identifies minimal pairs and homophones in regards to pitch accent.
    Words are sorted into output files based on various factors. Words can be sorted/filtered based on their frequency/occurence within a user's Anki deck.

    While certainly not a silver bullet in regards to pitch accent, it is designed to save time memorizing words.
    As always, a tool like this should be supplemented with heavy immersion to see real effects.

    A full tutorial can be found on the repository README: https://github.com/MtGambelOak/PitchParse

    Author: MtGambelOak
    Date: 10/2/24
    Ver: 1.0
 */


using System.Text;

namespace PitchParse
{
    internal class PitchParse
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            try
            {
                // Read settings from file. For explanation of settings see video
                string settings = File.ReadAllText("Inputs/settings.txt");
                string[] settingsScan = settings.Split('\n');

                bool readFromOld = settingsScan[1].ElementAt(settingsScan[1].Length - 2) == 'Y';
                bool writeToOld = settingsScan[2].ElementAt(settingsScan[2].Length - 2) == 'Y';
                bool sortBySecondaryPitch = settingsScan[3].ElementAt(settingsScan[3].Length - 2) == 'Y';
                bool sortByMoraOrCharCount = settingsScan[4].ElementAt(settingsScan[4].Length - 2) == 'Y';
                bool lumpKifuku = settingsScan[5].ElementAt(settingsScan[5].Length - 2) == 'Y';
                bool flagExceptions = settingsScan[6].ElementAt(settingsScan[6].Length - 2) == 'Y';
                bool ankiMode = settingsScan[7].ElementAt(settingsScan[7].Length - 2) == 'Y';
                settingsScan[8] = settingsScan[8].Replace('、', ',');
                settingsScan[8] = settingsScan[8].Substring(0, settingsScan[8].Length - 1);
                HashSet<string> fields = settingsScan[8].Substring(settingsScan[8].IndexOf(':') + 2).Split(',').ToHashSet();

                bool freqOnly = settingsScan[11].ElementAt(settingsScan[11].Length - 2) == 'Y';
                bool ignoreFreqs = settingsScan[12].ElementAt(settingsScan[12].Length - 2) == 'Y';
                if (fields.Count == 0 && !(freqOnly || ignoreFreqs))
                    Console.WriteLine("No fields to be scanned were added! Make sure to add them if you're scanning from an Anki collection!");
                bool showFrequency = settingsScan[13].ElementAt(settingsScan[13].Length - 2) == 'Y';
                bool checkReadings = settingsScan[14].ElementAt(settingsScan[14].Length - 2) == 'Y';
                int maxFrequency = 30000;
                if (Int32.TryParse(settingsScan[15].Substring(settingsScan[15].IndexOf(':') + 1).Trim(), out int maxFreqValue) && maxFreqValue >= 0)
                    maxFrequency = maxFreqValue;

                bool morePairs = settingsScan[18].ElementAt(settingsScan[18].Length - 2) == 'Y';

                bool sortStems = settingsScan[21].ElementAt(settingsScan[21].Length - 2) == 'Y';
                bool long4Mora = settingsScan[22].ElementAt(settingsScan[22].Length - 2) == 'Y';
                settingsScan[23] = settingsScan[23].Replace('、', ',');
                settingsScan[23] = settingsScan[23].Substring(0, settingsScan[23].Length - 1);
                List<string> filterEndings = settingsScan[23].Substring(settingsScan[23].IndexOf(':') + 2).Split(',').ToList();

                bool allAdjectives = settingsScan[26].ElementAt(settingsScan[26].Length - 2) == 'Y';
                bool tsuVerbs = settingsScan[27].ElementAt(settingsScan[27].Length - 1) == 'Y';

                string accentTarget = "accents.txt";
                if (File.Exists("Inputs/combinedAccents.txt"))
                    accentTarget = "combinedAccents.txt";
                else if (File.Exists("Inputs/nhk2016.txt"))
                    accentTarget = "nhk2016.txt";
                else if (File.Exists("Inputs/nhkAccents.txt"))
                    accentTarget = "nhkAccents.txt";

                // Gets words that the user knows from Anki collection if not done yet. A user "knows" a sentence if it is present in the fields they want to scan,
                // verbs and adjectives also are scanned for their possible conjugations
                // For each card, performs probably close to a million linear searches. So it takes a very long time, only need to do it once though
                List<char> endings = new List<char>() { 'る', 'す', 'く', 'ぐ', 'ぶ', 'つ', 'む', 'う', 'ぬ' };
                if (!File.Exists("Inputs/words.txt") && !freqOnly)
                {
                    Console.WriteLine("words.txt not found, finding words now...");
                    if (File.Exists("Inputs/cards.txt"))
                    {
                        string read = File.ReadAllText("Inputs/" + accentTarget);
                        string[] readTotal = read.Split('\n');

                        string cards = File.ReadAllText("Inputs/cards.txt");
                        string[] cardsTotal = cards.Split('\n');

                        HashSet<string> potWords = new HashSet<string>();

                        HashSet<string> words = new HashSet<string>();

                        for (int i = fields.Count; i < cardsTotal.Length; i++)
                            if (cardsTotal[i].Length > 0 && fields.Contains(cardsTotal[i].Substring(0, cardsTotal[i].Length - 1)))
                                generateWords(potWords, cardsTotal[i + 1]);

                        foreach (string word in readTotal)
                        {
                            string[] parts = word.Split("\t");
                            if (parts.Length == 3 && parts[0].Length > 0)
                            {
                                HashSet<string> conjugations = new HashSet<string>() { parts[0] };
                                if (parts[0].Last() == 'い')
                                    generateAdjConjugations(conjugations, parts[0]);
                                else if (endings.Contains(parts[0].Last()))
                                    generateVerbConjugations(conjugations, parts[0]);
                                foreach (string conjugation in conjugations)
                                {
                                    if (potWords.Contains(conjugation))
                                        words.Add(parts[0]);
                                }
                            }
                        }

                        StringBuilder wordsBuilder = new StringBuilder();

                        foreach (string word in words)
                            wordsBuilder.AppendLine(word);

                        File.WriteAllText("Inputs/words.txt", wordsBuilder.ToString());
                        Console.WriteLine("Words found in " + timer.ElapsedMilliseconds / 1000f + " seconds.");
                    }
                    else
                    {
                        Console.WriteLine("cards.txt was not found, forcing frequency only mode");
                        freqOnly = true;
                    }
                }

                string accents = File.ReadAllText("Inputs/" + accentTarget);
                string[] accentsTotal = accents.Split('\n');

                // Find possible verb stems - pretty much just the "i" version of sounds ending with a "u" sound.
                // Save the pitch accent "expected" of the noun gotten from the stem to see if its an exception
                HashSet<string> verbStems = new HashSet<string>();
                HashSet<string> verbStemsnr = new HashSet<string>();
                Dictionary<string, int> expectedStemPitches = new Dictionary<string, int>();

                Console.WriteLine("Finding verb stems");
                foreach (string line in accentsTotal)
                {
                    string[] parts = line.Split((char)9);
                    if (parts.Length == 3 && parts[0].Length > 0 && parts[1].Length > 0)
                    {
                        if (endings.Contains(parts[0].Last()))
                        {
                            verbStems.Add(parts[0].Remove(parts[0].Length - 1) + getISound(parts[0].Last()) + " " + parts[1].Remove(parts[1].Length - 1) + getISound(parts[1].Last()));
                            verbStemsnr.Add(parts[0].Remove(parts[0].Length - 1) + getISound(parts[0].Last()));
                            int expectedPitch = 0;
                            if (parts[2].First() != '0' && parts[2].First() != '(')
                                expectedPitch = moraCount(parts[1]);
                            if (!expectedStemPitches.ContainsKey(parts[0].Remove(parts[0].Length - 1) + getISound(parts[0].Last()) + " " + parts[1].Remove(parts[1].Length - 1) + getISound(parts[1].Last())))
                                expectedStemPitches.Add(parts[0].Remove(parts[0].Length - 1) + getISound(parts[0].Last()) + " " + parts[1].Remove(parts[1].Length - 1) + getISound(parts[1].Last()), expectedPitch);
                        }
                        if (parts[0].Last() == 'る')
                        {
                            verbStems.Add(parts[0].Remove(parts[0].Length - 1) + " " + parts[1].Remove(parts[1].Length - 1));
                            verbStemsnr.Add(parts[0].Remove(parts[0].Length - 1));
                            int expectedPitch = 0;
                            if (parts[2].First() != '0' && parts[2].First() != '(')
                                expectedPitch = moraCount(parts[1]) - 1;
                            if (!expectedStemPitches.ContainsKey(parts[0].Remove(parts[0].Length - 1) + " " + parts[1].Remove(parts[1].Length - 1)))
                                expectedStemPitches.Add(parts[0].Remove(parts[0].Length - 1) + " " + parts[1].Remove(parts[1].Length - 1), expectedPitch);
                        }
                    }
                }

                HashSet<string> foundNouns = new HashSet<string>();
                HashSet<string> foundAdjectives = new HashSet<string>();
                HashSet<string> foundVerbs = new HashSet<string>();

                Dictionary<string, int> foundNounFrequency = new Dictionary<string, int>();
                Dictionary<string, int> foundAdjectiveFrequency = new Dictionary<string, int>();
                Dictionary<string, int> foundVerbFrequency = new Dictionary<string, int>();

                Dictionary<string, int> timesFound = new Dictionary<string, int>();

                string wordsTotal;

                if (freqOnly || ignoreFreqs)
                    wordsTotal = File.ReadAllText("Inputs/" + accentTarget);
                else
                    wordsTotal = File.ReadAllText("Inputs/words.txt");
                string[] wordsScan = wordsTotal.Split('\n');

                // Load words based on words.txt or accents file based on settings; note all words ending with an "u" sound are put into verbs, couldn't find another way
                Console.WriteLine("Getting found words");
                if (freqOnly)
                    Console.WriteLine("(Will filter ones below frequency cutoff)");
                for (int i = 0; i < wordsScan.Length; i++)
                {
                    if (wordsScan[i].Length > 0)
                    {
                        wordsScan[i] = wordsScan[i].Split("\t")[0];
                        if (wordsScan[i].Length > 0 && (wordsScan[i].EndsWith(' ') || wordsScan[i].EndsWith('\r')))
                            wordsScan[i] = wordsScan[i].Substring(0, wordsScan[i].Length - 1);
                        if (wordsScan[i].Length > 0)
                        {
                            if (endings.Contains(wordsScan[i].Last()) && !(allHiragana(wordsScan[i]) || allKatakana(wordsScan[i])))
                                foundVerbs.Add(wordsScan[i]);
                            else if (wordsScan[i].Last() == 'い' && !verbStemsnr.Contains(wordsScan[i]) && !(allHiragana(wordsScan[i]) || allKatakana(wordsScan[i])))
                                foundAdjectives.Add(wordsScan[i]);
                            else
                                if (wordsScan[i].Length > 0)
                                foundNouns.Add(wordsScan[i]);
                        }
                    }
                }


                // Loads frequencies from frequency files. Note that frequency values are averaged, and if the same sentence appears twice in a frequency file
                // (esp if not distinguishing frequency in settings) the first found value in a file is used.
                Console.WriteLine("Getting frequencies");
                string[] files = Directory.GetFiles("Inputs/Frequencies");
                if (files.Length == 0)
                    Console.WriteLine("No frequency files found! You should add some!");
                foreach (string file in files)
                {
                    string fileTotal = File.ReadAllText(file);
                    string[] fileScan = fileTotal.Split('\n');

                    HashSet<string> alreadyLookedAt = new HashSet<string>();

                    foreach (string line in fileScan)
                    {
                        string[] parts = line.Split(' ');
                        if ((parts.Length == 2 || parts.Length == 3) && !alreadyLookedAt.Contains(parts[0]))
                        {
                            string reading = "";
                            if (checkReadings)
                                reading = " " + parts[1];
                            if (foundNounFrequency.TryGetValue(parts[0] + reading, out int nounFreq))
                            {
                                int newFreq = nounFreq + Int32.Parse(parts.Last());
                                foundNounFrequency.Remove(parts[0] + reading);
                                foundNounFrequency.Add(parts[0] + reading, newFreq);
                            }
                            else if (!(parts[0].Last() == 'い' && !verbStems.Contains(parts[0] + " " + parts[1])))
                                foundNounFrequency.Add(parts[0] + reading, Int32.Parse(parts.Last()));
                            if (foundAdjectiveFrequency.TryGetValue(parts[0] + reading, out int adjFreq))
                            {
                                int newFreq = adjFreq + Int32.Parse(parts.Last());
                                foundAdjectiveFrequency.Remove(parts[0] + reading);
                                foundAdjectiveFrequency.Add(parts[0] + reading, newFreq);
                            }
                            else if (parts[0].Last() == 'い' && !verbStems.Contains(parts[0] + " " + parts[1]))
                                foundAdjectiveFrequency.Add(parts[0] + reading, Int32.Parse(parts.Last()));
                            if (foundVerbFrequency.TryGetValue(parts[0] + reading, out int verbFreq))
                            {
                                int newFreq = verbFreq + Int32.Parse(parts.Last());
                                foundVerbFrequency.Remove(parts[0] + reading);
                                foundVerbFrequency.Add(parts[0] + reading, newFreq);
                            }
                            else if (endings.Contains(parts[0].Last()))
                                foundVerbFrequency.Add(parts[0] + reading, Int32.Parse(parts.Last()));
                            if (timesFound.TryGetValue(parts[0] + reading, out int times))
                            {
                                timesFound.Remove(parts[0] + reading);
                                timesFound.Add(parts[0] + reading, times + 1);
                            }
                            else
                                timesFound.Add(parts[0] + reading, 1);
                            if (!alreadyLookedAt.Contains(parts[0] + reading))
                                alreadyLookedAt.Add(parts[0] + reading);
                        }
                    }
                }
                // Average values
                foreach (var pair in timesFound)
                {
                    if (foundNounFrequency.TryGetValue(pair.Key, out int nounFreq))
                    {
                        int newFreq;
                        newFreq = nounFreq / pair.Value;
                        foundNounFrequency.Remove(pair.Key);
                        foundNounFrequency.Add(pair.Key, newFreq);
                    }
                    if (foundAdjectiveFrequency.TryGetValue(pair.Key, out int adjFreq))
                    {
                        int newFreq;
                        newFreq = adjFreq / pair.Value;
                        foundAdjectiveFrequency.Remove(pair.Key);
                        foundAdjectiveFrequency.Add(pair.Key, newFreq);
                    }
                    if (foundVerbFrequency.TryGetValue(pair.Key, out int verbFreq))
                    {
                        int newFreq;
                        newFreq = verbFreq / pair.Value;
                        foundVerbFrequency.Remove(pair.Key);
                        foundVerbFrequency.Add(pair.Key, newFreq);
                    }
                }


                // What words have been processed in a previous session and may need to be filtered?
                HashSet<string> oldWords = new HashSet<string>();
                // String builder for eventual old words output
                StringBuilder oldWordsBuilder = new StringBuilder();

                // Read from the previous old words file, if reading flag add to old words (filters output), otherwise add to builder so data persists
                try
                {
                    string oldTotal = File.ReadAllText("Inputs/old words.txt");
                    string[] oldScan = oldTotal.Split('\n');

                    for (int i = 0; i < oldScan.Length; i++)
                        if (oldScan[i].Length > 0)
                        {
                            oldWords.Add(oldScan[i].Split(" ")[0]);
                            oldWordsBuilder.Append(oldScan[i].Split(" ")[0] + "\n");
                        }
                }
                catch (Exception) { Console.WriteLine("Old words was not found."); }

                // Repeat above, but for minimal pairs
                HashSet<string> oldPairs = new HashSet<string>();
                StringBuilder oldPairsBuilder = new StringBuilder();

                try
                {
                    string oldTotal = File.ReadAllText("Inputs/old minimal pairs.txt");
                    string[] oldScan = oldTotal.Split('\n');

                    for (int i = 0; i < oldScan.Length; i++)
                    {
                        if (oldScan[i].Length > 0)
                        {
                            oldPairs.Add(oldScan[i]);
                            oldPairsBuilder.Append(oldScan[i] + "\n");
                        }
                    }
                }
                catch (Exception) { Console.WriteLine("Old minimal pairs was not found."); }

                // Lots of dicts for different categories lol
                SortedDictionary<string, string> stemList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> fourMoraNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> threeMoraNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> twoMoraNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> oneMoraNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> compoundNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> miscNounList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> multiWordList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> katakanaList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> hiraganaList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> adjectiveList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> verbList = new SortedDictionary<string, string>();
                SortedDictionary<string, string> compoundVerbList = new SortedDictionary<string, string>();

                Dictionary<string, Tuple<List<string>, List<string>>> minimalPairs = new Dictionary<string, Tuple<List<string>, List<string>>>();

                HashSet<string> osounds = new HashSet<string>() { "お", "こ", "ご", "そ", "ぞ", "と", "ど", "の", "ほ", "ぼ", "ぽ", "も", "よ", "ろ" };

                string total = File.ReadAllText("Inputs/" + accentTarget);
                string[] scan = total.Split('\n');

                // Real quick get stem compounds for diff behavior
                HashSet<string> compoundStems = new HashSet<string>();
                foreach (string scanLine in scan)
                {
                    string line = scanLine.Replace("\r", "");
                    string[] parts = line.Split((char)9);
                    if (parts.Length == 3)
                    {
                        if (foundVerbs.Contains(parts[0]) && parts[1].Length > 0)
                        {
                            bool compoundFlag = false;
                            for (int i = parts[0].Length - 2; i >= 0; i--)
                                if (kanjiCount(parts[0].Substring(i)) > 0 && foundVerbs.Contains(parts[0].Substring(i)) && (foundVerbs.Contains(parts[0].Substring(0, i)) || foundNouns.Contains(parts[0].Substring(0, i)) || verbStemsnr.Contains(parts[0].Substring(0, i))))
                                    compoundFlag = true;
                            if (compoundFlag)
                            {
                                compoundStems.Add(parts[0].Remove(parts[0].Length - 1) + getISound(parts[0].Last()) + " " + parts[1].Remove(parts[1].Length - 1) + getISound(parts[1].Last()));
                                if (parts[0].Last() == 'る')
                                    compoundStems.Add(parts[0].Remove(parts[0].Length - 1) + " " + parts[1].Remove(parts[1].Length - 1));
                            }
                        }
                    }
                }

                // Now loop through accent data using the info we have
                Console.WriteLine("Doing pitch calculations and sentence type sorting");
                foreach (string scanLine in scan)
                {
                    string line = scanLine.Replace("\r", "");
                    // parts[0] is the sentence, i.e. 走る. parts[1] is the reading, i.e. はしる. if hiragana/katakana sentence this field is empty. parts[2] is the pitch data, i.e. 2
                    string[] parts = line.Split((char)9);
                    {
                        if (parts.Length == 3)
                        {
                            string reading = " " + parts[1];
                            if (parts[1].Length == 0)
                                reading = " " + parts[0];
                            reading = reading.Replace("・", "");
                            if (!checkReadings)
                                reading = "";
                            bool lowFreqFlag = false;
                            bool foundFlag = false;
                            if (foundNounFrequency.TryGetValue(parts[0] + reading, out var nfreq))
                            {
                                foundFlag = true;
                                if (nfreq >= maxFrequency)
                                    lowFreqFlag = true;
                            }
                            if (foundAdjectiveFrequency.TryGetValue(parts[0] + reading, out var afreq))
                            {
                                foundFlag = true;
                                if (afreq >= maxFrequency)
                                    lowFreqFlag = true;
                            }
                            if (foundVerbFrequency.TryGetValue(parts[0] + reading, out var vfreq))
                            {
                                foundFlag = true;
                                if (vfreq >= maxFrequency)
                                    lowFreqFlag = true;
                            }

                            if (lowFreqFlag || !foundFlag)
                            {
                                if (ignoreFreqs)
                                {
                                    if (nfreq == 0)
                                        nfreq = maxFreqValue + 1;
                                    if (afreq == 0)
                                        afreq = maxFreqValue + 1;
                                    if (vfreq == 0)
                                        vfreq = maxFreqValue + 1;
                                }
                                else
                                    continue;
                            }

                            // If the pair was added to nouns, adj, or verbs list, do minimal pairs logic.
                            if (foundNouns.Contains(parts[0]) || foundAdjectives.Contains(parts[0]) || foundVerbs.Contains(parts[0]))
                            {
                                // Normalize idential pronunciations
                                string normalized = parts[1].Replace('づ', 'ず');
                                normalized = normalized.Replace('ぢ', 'じ');
                                // ほお and ほう for ex are pronounced the same in modern Japanese so normalize just in case for words like 頬
                                foreach (string osound in osounds)
                                    normalized = normalized.Replace(osound + "お", osound + "う");
                                // Maybe a better way to do this, but just basically add another entry if the reading already existed, or create one if it did not.
                                if (minimalPairs.TryGetValue(normalized, out var pitchValues) && !allHiragana(parts[0]))
                                {
                                    pitchValues.Item1.Add(parts[0] + " " + parts[2]);
                                    pitchValues.Item2.Add(parts[2]);
                                    pitchValues = new Tuple<List<string>, List<string>>(pitchValues.Item1, pitchValues.Item2);
                                    minimalPairs.Remove(normalized);
                                    minimalPairs.Add(normalized, pitchValues);
                                }
                                else if (normalized.Length > 0 && !allHiragana(parts[0]))
                                {
                                    Tuple<List<string>, List<string>> newPitchList = new Tuple<List<string>, List<string>>(new List<String>(), new List<String>());
                                    string addReading = normalized + " ";
                                    if (ankiMode)
                                        addReading = "";
                                    newPitchList.Item1.Add(addReading + parts[0] + " " + parts[2]);
                                    newPitchList.Item2.Add(parts[2]);
                                    minimalPairs.Add(normalized, newPitchList);
                                }
                            }
                            // Now just filter into different categories
                            if (foundNouns.Contains(parts[0]))
                            {
                                if (parts[1].Contains("・"))
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !multiWordList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        multiWordList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (allKatakana(parts[0]))
                                {
                                    if (parts[1].Length == 0)
                                    {
                                        HashSet<char> specialSounds = new HashSet<char>() { 'イ', 'ー', 'ッ', 'ン' };
                                        if (!oldWords.Contains(parts[0]) || !readFromOld)
                                        {
                                            string irregular = "";
                                            if (Int32.TryParse(parts[2].First() + "", out int pitch))
                                            {
                                                if (pitch != 0 && moraCount(parts[0]) > 2 && pitch != moraCount(parts[0]) - 2)
                                                {
                                                    if (!specialSounds.Contains(nthMora(parts[0], pitch + 1)) || Math.Abs((pitch - (moraCount(parts[0]) - 2))) > 1)
                                                    {
                                                        if (flagExceptions)
                                                            irregular += " irregular downstep ";
                                                    }
                                                }
                                                else if (moraCount(parts[0]) == 3 && parts[0].Last() == 'ー' && pitch != 2)
                                                {
                                                    if (flagExceptions)
                                                        irregular += " irregular ー pair ";
                                                }
                                            }
                                            katakanaList.Add(parts[0] + " " + parts[1] + " " + parts[2] + irregular + " freq " + nfreq + "\n", parts[2]);
                                            if (writeToOld && !oldWords.Contains(parts[0]))
                                                oldWordsBuilder.Append(parts[0] + "\n");
                                        }
                                    }
                                }
                                else if (allHiragana(parts[0]))
                                {
                                    if (parts[1].Length == 0)
                                    {
                                        if (!oldWords.Contains(parts[0]) || !readFromOld)
                                        {
                                            string regular = "";
                                            // My god that's ugly logic, basically just if it's a sentence like さっぱり, make sure it's longer than 2 mora, ends with り, has downstep on second to last mora, and is not a 4 mora sentence without っ/ん on the second mora
                                            if (moraCount(parts[0]) > 2 && parts[0].Last() == 'り' && parts[2].First() == (moraCount(parts[0]) - 1).ToString().First() && !(moraCount(parts[0]) == 4 && (nthMora(parts[0], 2) != 'っ' && nthMora(parts[0], 2) != 'ん')))
                                                regular = " standard り mimetic/onomatopoeia";
                                            if (moraCount(parts[0]) == 4 && (nthMora(parts[0], 1) + "" + nthMora(parts[0], 2)) == (nthMora(parts[0], 3) + "" + nthMora(parts[0], 4)) && (parts[2].Split(',')[0].Contains('1')))
                                                regular = " standard reduplication";
                                            hiraganaList.Add(parts[0] + " " + parts[1] + " " + parts[2] + regular + " freq " + nfreq + "\n", parts[2]);
                                            if (writeToOld && !oldWords.Contains(parts[0]))
                                                oldWordsBuilder.Append(parts[0] + "\n");
                                        }
                                    }
                                }
                                else if (sortStems && verbStems.Contains(parts[0] + " " + parts[1]) && kanjiCount(parts[0]) != parts[0].Length)
                                {
                                    string irregular = "";
                                    if (expectedStemPitches.TryGetValue(parts[0] + " " + parts[1], out int expectedPitch))
                                    {
                                        if (!parts[2].Contains(expectedPitch.ToString()))
                                            irregular = " irregular accent! expected " + expectedPitch + ". ";
                                        if (compoundStems.Contains(parts[0] + " " + parts[1]))
                                        {
                                            if (!parts[2].Contains('0'))
                                                irregular += "(or 0).";
                                            else
                                                irregular = "";
                                        }
                                    }
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !stemList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        stemList.Add(parts[0] + " " + parts[1] + " " + parts[2] + irregular + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (moraCount(parts[1]) == 4 && !(!long4Mora && moraCount(parts[0]) > 2))
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !fourMoraNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        fourMoraNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (moraCount(parts[1]) == 3 && !(!long4Mora && moraCount(parts[0]) > 2))
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !threeMoraNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        threeMoraNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (moraCount(parts[1]) == 2)
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !twoMoraNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        twoMoraNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (moraCount(parts[1]) == 1)
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !oneMoraNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        oneMoraNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                                else if (moraCount(parts[1]) > 4)
                                {
                                    bool filterFlag = false;
                                    for (int i = 0; i < filterEndings.Count - 1; i += 2)
                                        if (parts[0].EndsWith(filterEndings.ElementAt(i)) && parts[1].EndsWith(filterEndings.ElementAt(i + 1)) && parts[2].First() == '0')
                                            filterFlag = true;
                                    if (!filterFlag)
                                    {
                                        if ((!oldWords.Contains(parts[0]) || !readFromOld) && !compoundNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                        {
                                            compoundNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                            if (writeToOld && !oldWords.Contains(parts[0]))
                                                oldWordsBuilder.Append(parts[0] + "\n");
                                        }
                                    }
                                }
                                else
                                {
                                    bool filterFlag = false;
                                    for (int i = 0; i < filterEndings.Count - 1; i += 2)
                                        if (parts[0].EndsWith(filterEndings.ElementAt(i)) && parts[1].EndsWith(filterEndings.ElementAt(i + 1)) && parts[2].First() == '0')
                                            filterFlag = true;
                                    if (!filterFlag)
                                    {
                                        if ((!oldWords.Contains(parts[0]) || !readFromOld) && !miscNounList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                        {
                                            miscNounList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n", parts[2]);
                                            if (writeToOld && !oldWords.Contains(parts[0]))
                                                oldWordsBuilder.Append(parts[0] + "\n");
                                        }
                                    }
                                }
                            }
                            if (foundAdjectives.Contains(parts[0]) || (allAdjectives && parts[0].Last() == 'い'))
                            {
                                if (parts[0].Length > 1)
                                {
                                    if ((!oldWords.Contains(parts[0]) || !readFromOld) && !adjectiveList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                    {
                                        string irregular = "";
                                        if (Int32.TryParse(parts[2].First() + "", out int pitch))
                                            if (pitch != 0 && pitch != moraCount(parts[1]) - 1 && flagExceptions && parts[1].Length > 1)
                                                irregular = " shifted downstep ";
                                        adjectiveList.Add(parts[0] + " " + parts[1] + " " + parts[2] + irregular + " freq " + afreq + "\n", parts[2]);
                                        if (writeToOld && !oldWords.Contains(parts[0]))
                                            oldWordsBuilder.Append(parts[0] + "\n");
                                    }
                                }
                            }
                            if (foundVerbs.Contains(parts[0]))
                            {
                                // Start by reading 2 chars from the back. If the last segment of the string is a verb,
                                // and the first segment of the string is a verb stem, noun, or verb, we know it's a compound.
                                bool compoundFlag = false;
                                for (int i = parts[0].Length - 2; i >= 0; i--)
                                    if (kanjiCount(parts[0].Substring(i)) > 0 && foundVerbs.Contains(parts[0].Substring(i)) && (foundVerbs.Contains(parts[0].Substring(0, i)) || foundNouns.Contains(parts[0].Substring(0, i)) || verbStems.Contains(parts[0].Substring(0, i))))
                                        compoundFlag = true;
                                if (!compoundFlag)
                                {
                                    List<char> iSounds = new List<char>() { 'り', 'し', 'き', 'ぎ', 'び', 'ち', 'み', 'い', 'に' };

                                    if (parts[0].Length > 1)
                                    {
                                        if ((!oldWords.Contains(parts[0]) || !readFromOld) && !verbList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                        {
                                            if (!foundAdjectives.Contains(parts[0].Remove(parts[0].Length - 1) + "い") && (tsuVerbs || parts[0].Last() != 'つ'))
                                            {
                                                string irregular = "";
                                                if (Int32.TryParse(parts[2].First() + "", out int pitch) && parts[1].Length > 0)
                                                {
                                                    if (pitch != 0 && pitch != moraCount(parts[1]) - 1 && flagExceptions && parts[1].Length > 1)
                                                        irregular += " shifted downstep ";
                                                    if (parts[1].Length > 1 && parts[1].Last() == 'ぶ' && pitch != 0 && flagExceptions)
                                                        irregular += " accented ぶ verb ";
                                                    if (parts[1].Length == 3 && iSounds.Contains(nthMora(parts[1], 2)) && pitch == 0 && flagExceptions)
                                                        irregular += " flat i-sound verb ";
                                                }
                                                verbList.Add(parts[0] + " " + parts[1] + " " + parts[2] + irregular + " freq " + vfreq + "\n", parts[2]);
                                                if (writeToOld && !oldWords.Contains(parts[0]))
                                                    oldWordsBuilder.Append(parts[0] + "\n");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (parts[0].Length > 1)
                                    {
                                        if ((!oldWords.Contains(parts[0]) || !readFromOld) && !compoundVerbList.ContainsKey(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + nfreq + "\n"))
                                        {
                                            if (!foundAdjectives.Contains(parts[0].Remove(parts[0].Length - 1) + "い"))
                                            {
                                                compoundVerbList.Add(parts[0] + " " + parts[1] + " " + parts[2] + " freq " + vfreq + "\n", parts[2]);
                                                if (writeToOld && !oldWords.Contains(parts[0]))
                                                    oldWordsBuilder.Append(parts[0] + "\n");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                StringBuilder stemNounBuilder = new StringBuilder();
                StringBuilder belowFourMoraNounBuilder = new StringBuilder();
                StringBuilder longAndCompoundNounBuilder = new StringBuilder();
                StringBuilder hiraganaAndKatakanaBuilder = new StringBuilder();
                StringBuilder adjectiveAndVerbBuilder = new StringBuilder();
                StringBuilder minimalListBuilder = new StringBuilder();
                StringBuilder homophonesBuilder = new StringBuilder();
                StringBuilder wordBuilder = new StringBuilder();

                CompareFrequenciesAscending compare = new CompareFrequenciesAscending();

                Console.WriteLine("Sorting and writing to files");

                populateBuilder(stemList, stemNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 1);

                belowFourMoraNounBuilder.Append("Four Mora Nouns:\n");
                populateBuilder(fourMoraNounList, belowFourMoraNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency);
                belowFourMoraNounBuilder.Append("\nThree Mora Nouns:\n");
                populateBuilder(threeMoraNounList, belowFourMoraNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency);
                belowFourMoraNounBuilder.Append("\nTwo Mora Nouns:\n");
                populateBuilder(twoMoraNounList, belowFourMoraNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency);
                belowFourMoraNounBuilder.Append("\nOne Mora Nouns:\n");
                populateBuilder(oneMoraNounList, belowFourMoraNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency);

                longAndCompoundNounBuilder.Append("Long Nouns:\n");
                populateBuilder(compoundNounList, longAndCompoundNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 1);
                longAndCompoundNounBuilder.Append("\nMiscellaneous Nouns:\n");
                populateBuilder(miscNounList, longAndCompoundNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 1);
                longAndCompoundNounBuilder.Append("\nMultiple Word Nouns:\n");
                populateBuilder(multiWordList, longAndCompoundNounBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 1);

                hiraganaAndKatakanaBuilder.Append("Hiragana Words:\n");
                populateBuilder(hiraganaList, hiraganaAndKatakanaBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 0);
                hiraganaAndKatakanaBuilder.Append("\nKatakana Words:\n");
                populateBuilder(katakanaList, hiraganaAndKatakanaBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, 0);

                adjectiveAndVerbBuilder.Append("I-Adjectives:\n");
                populateBuilder(adjectiveList, adjectiveAndVerbBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, lumpKifuku);
                adjectiveAndVerbBuilder.Append("\nVerbs:\n");
                populateBuilder(verbList, adjectiveAndVerbBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, lumpKifuku);
                adjectiveAndVerbBuilder.Append("\nCompound Verbs:\n");
                populateBuilder(compoundVerbList, adjectiveAndVerbBuilder, compare, sortByMoraOrCharCount, sortBySecondaryPitch, showFrequency, lumpKifuku);

                foreach (var word in foundNouns)
                    wordBuilder.AppendLine(word);
                foreach (var word in foundAdjectives)
                    wordBuilder.AppendLine(word);
                foreach (var word in foundVerbs)
                    wordBuilder.AppendLine(word);

                HashSet<string> pitchSymbols = new HashSet<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "," };

                // Minimal pairs are not sorted, logic is quite messy for showing them though, so I break it up into smaller pieces
                foreach (var group in minimalPairs.Values)
                {
                    // ... text parsing gets REAL ugly for anki mode and to make sure old files are written and read the same way regardless of anki mode. I would just not touch this part...
                    string line;
                    bool oneUnequalPair = group.Item2.Any(o => o.ElementAt(0) != group.Item2[0].ElementAt(0));
                    bool oneOrMorelongGroup = group.Item2.Any(o => o.Length != 1);
                    // Thus, either there is one pair of pitch values in the reading group that have different primary pitch values, or if more pitches are requested, it's simply enough
                    // for one pitch value to have multiple possible values, and have there be multiple words in the reading group.
                    if (oneUnequalPair || (oneOrMorelongGroup && group.Item2.Count > 1 && morePairs))
                    {
                        string tot = "";
                        line = "";
                        IOrderedEnumerable<string> pitches = group.Item1.OrderBy(kvp => kvp.Substring(kvp.LastIndexOf(" ")));
                        string oldCheck = "";
                        foreach (string entry in pitches)
                            oldCheck += entry + " ";
                        if (oldPairs.Contains(oldCheck) && readFromOld)
                            continue;
                        string number = pitches.ElementAt(0).Substring(pitches.ElementAt(0).IndexOf(' '));
                        for (int i = 0; i < pitches.Count(); i++)
                        {
                            string s = pitches.ElementAt(i);
                            if (ankiMode)
                            {
                                if (number.ElementAt(1) != s.Substring(s.IndexOf(' ')).ElementAt(1))
                                    s = "\n" + s;
                            }
                            number = pitches.ElementAt(i).Substring(pitches.ElementAt(i).LastIndexOf(' '));
                            string toAdd = pitches.ElementAt(i);
                            if (pitches.ElementAt(i).Count(c => c == ' ') == 2)
                                toAdd = pitches.ElementAt(i).Substring(pitches.ElementAt(i).IndexOf(' ') + 1);
                            line += (toAdd + " ");
                            tot += (s + " ");
                        }
                        foreach (string samePitch in tot.Split("\n"))
                        {
                            List<string> notThis = tot.Split("\n").ToList().FindAll(l => l != samePitch);
                            string pitchRemoved = samePitch;
                            if (ankiMode)
                            {
                                foreach (string toRepalce in pitchSymbols)
                                    pitchRemoved = pitchRemoved.Replace(toRepalce, "").Replace("  ", " ");
                                pitchRemoved = pitchRemoved.Replace(" ", "、");
                            }
                            minimalListBuilder.Append(pitchRemoved.Substring(0, pitchRemoved.Length - 1) + "\n");
                            for (int i = 0; i < notThis.Count; i++)
                            {
                                if (i != notThis.Count - 1)
                                    minimalListBuilder.Append("(" + notThis[i] + ")");
                                else
                                    minimalListBuilder.Append("(" + notThis[i].Substring(0, notThis[i].Length - 1) + ")");
                            }
                            if (ankiMode)
                                minimalListBuilder.Append("。\n\n");
                        }
                        if (writeToOld && !oldPairs.Contains(line + "\n"))
                            oldPairsBuilder.Append(line + "\n");
                    }
                    else if (!oneUnequalPair && group.Item1.Count > 1)
                    {
                        string oldCheck = "";
                        foreach (string entry in group.Item1)
                            oldCheck += entry + " ";
                        if (oldPairs.Contains(oldCheck) && readFromOld)
                            continue;
                        line = "";
                        char primaryPitch = group.Item1[0].Substring(group.Item1[0].LastIndexOf(" ") + 1).First();
                        for (int i = 0; i < group.Item1.Count; i++)
                        {
                            string s = group.Item1[i];
                            if (group.Item1[i].Substring(group.Item1[i].LastIndexOf(" ") + 1).Length == 1)
                                s = s.Substring(0, s.LastIndexOf(" "));
                            else
                                s = s.Substring(0, s.LastIndexOf(" ") + 1) + "(" + s.Substring(s.LastIndexOf(" ") + 1) + ")";
                            if (i == group.Item1.Count - 1)
                                s += " " + primaryPitch;
                            if (ankiMode && i == group.Item1.Count - 1)
                                homophonesBuilder.Append(s + "。");
                            else
                            {
                                if (ankiMode)
                                    homophonesBuilder.Append(s + "、");
                                else
                                    homophonesBuilder.Append(s + " ");
                            }
                            string toAdd = group.Item1[i];
                            if (group.Item1[i].Count(c => c == ' ') == 2)
                                toAdd = group.Item1[i].Substring(group.Item1[i].IndexOf(' ') + 1);
                            line += (toAdd + " ");
                        }
                        homophonesBuilder.Append("\n");
                        if (writeToOld && !oldPairs.Contains(line + "\n"))
                            oldPairsBuilder.Append(line + "\n");
                    }
                }

                // Write outputs to files
                File.WriteAllText("Outputs/stem nouns.txt", stemNounBuilder.ToString());
                File.WriteAllText("Outputs/1-4 mora nouns.txt", belowFourMoraNounBuilder.ToString());
                File.WriteAllText("Outputs/long and compound nouns.txt", longAndCompoundNounBuilder.ToString());
                File.WriteAllText("Outputs/hiragana and katakana words.txt", hiraganaAndKatakanaBuilder.ToString());
                File.WriteAllText("Outputs/adjectives and verbs.txt", adjectiveAndVerbBuilder.ToString());
                File.WriteAllText("Inputs/old words.txt", oldWordsBuilder.ToString());
                File.WriteAllText("Outputs/minimal pairs.txt", minimalListBuilder.ToString());
                File.WriteAllText("Outputs/homophones.txt", homophonesBuilder.ToString());
                if (!freqOnly && !ignoreFreqs)
                    File.WriteAllText("Inputs/words.txt", wordBuilder.ToString());

                // Write back to old files
                File.WriteAllText("Inputs/old words.txt", oldWordsBuilder.ToString());
                File.WriteAllText("Inputs/old minimal pairs.txt", oldPairsBuilder.ToString());

                timer.Stop();
                Console.WriteLine("Done in " + timer.ElapsedMilliseconds / 1000f + " seconds. Press any key to exit");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong during execution...");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        // Generates all possible words out of a string, by sort of generating a "power set". Returns all length 1 substrings + all length 2 substrings... etc
        static void generateWords(HashSet<string> words, string sentence)
        {
            for (int i = 1; i <= sentence.Length; i++)
                for (int k = 0; k <= sentence.Length - i; k++)
                    words.Add(sentence.Substring(k, i));
        }

        // Given a hash set, adds conjugations to it for each possible i-adj conjugation given the input.
        static void generateAdjConjugations(HashSet<string> conjugations, string input)
        {
            conjugations.Add(input.Substring(0, input.Length - 1) + "く");
            conjugations.Add(input.Substring(0, input.Length - 1) + "さ");
            conjugations.Add(input.Substring(0, input.Length - 1) + "く");
            conjugations.Add(input.Substring(0, input.Length - 1) + "き");
            conjugations.Add(input.Substring(0, input.Length - 1) + "げ");
        }

        // Given a hash set, adds conjugations to it for each possible verb conjugation given the input.
        static void generateVerbConjugations(HashSet<string> conjugations, string input)
        {
            char ending = input.Last();

            // Get past tense and te form. Just need to add first char, rest will be found
            if (ending == 'す')
                conjugations.Add(input.Substring(0, input.Length - 1) + "し");
            if (ending == 'く' || ending == 'ぐ')
                conjugations.Add(input.Substring(0, input.Length - 1) + "い");
            if (ending == 'む' || ending == 'ぶ' || ending == 'ぬ')
                conjugations.Add(input.Substring(0, input.Length - 1) + "ん");
            if (ending == 'る' || ending == 'う' || ending == 'つ')
                conjugations.Add(input.Substring(0, input.Length - 1) + "っ");

            // Gets neg, stem, potential. Again only need to add first char
            conjugations.Add(input.Substring(0, input.Length - 1) + getASound(ending));
            conjugations.Add(input.Substring(0, input.Length - 1) + getISound(ending));
            conjugations.Add(input.Substring(0, input.Length - 1) + getESound(ending));

            // This should take care of ichidan
            if (ending == 'る')
                conjugations.Add(input.Substring(0, input.Length - 1));

            // This actually proably covers everything, even rare conjugations like ず、ぬ are built upon ones we alr included,
            // And any sentence in a collection which only shows up once in some weird conjugation is probably nothing to worry about.
        }

        // Returns the "あ" version of a "う" sound, i.e. く -> か
        static char getASound(char sound)
        {
            // Offsets to get sound is not the same for every sound, but there are patterns that make it easier
            if (sound == 'う')
                return 'わ';
            else if (sound == 'す' || sound == 'く' || sound == 'ぐ')
                return (char)(sound - 4);
            else if (sound == 'ぶ')
                return 'ば';
            else if (sound == 'つ')
                return 'た';
            else
                return (char)(sound - 2);
        }

        // Returns the "い" version of a "う" sound, i.e. く -> き
        static char getISound(char sound)
        {
            // Offsets to get sound is not the same for every sound, but there are patterns that make it easier
            if (sound == 'う')
                return 'い';
            else if (sound == 'す' || sound == 'く' || sound == 'ぐ')
                return (char)(sound - 2);
            else if (sound == 'ぶ')
                return 'び';
            else if (sound == 'つ')
                return 'ち';
            else
                return (char)(sound - 1);
        }

        // Returns the "え" version of a "う" sound, i.e. く -> け
        static char getESound(char sound)
        {
            // Offsets to get sound is not the same for every sound, but there are patterns that make it easier
            if (sound == 'う')
                return 'え';
            else if (sound == 'す' || sound == 'く' || sound == 'ぐ')
                return (char)(sound + 2);
            else if (sound == 'ぶ')
                return 'べ';
            else if (sound == 'つ')
                return 'て';
            else
                return (char)(sound + 1);
        }

        // Returns how many morae are in the pair. Does this by ignoring small characters
        static int moraCount(string input)
        {
            HashSet<char> smallChars = new HashSet<char>() { 'ゃ', 'ゅ', 'ょ', 'ャ', 'ュ', 'ョ', 'ァ', 'ィ', 'ゥ', 'ェ', 'ォ' };
            // Just count and ignore small chars
            int count = 0;
            foreach (char c in input)
            {
                if (!smallChars.Contains(c))
                    count++;
            }
            return count;
        }

        // Returns the "nth" mora in the pair, but technically only returns the character. So if the second mora is しょ, し is returned
        static char nthMora(string input, int n)
        {
            HashSet<char> smallChars = new HashSet<char>() { 'ゃ', 'ゅ', 'ょ', 'ャ', 'ュ', 'ョ', 'ァ', 'ィ', 'ゥ', 'ェ', 'ォ' };
            // Similar to moraCount
            int count = 0;
            foreach (char c in input)
            {
                if (!smallChars.Contains(c))
                    count++;
                if (count == n)
                    return c;
            }
            return '\0';
        }

        // Returns how many kanjis are in the pair. Or more exactly, how many characters are NOT kana.
        static int kanjiCount(string input)
        {
            // Hiragana range 12352-12447
            // Katakana range 12448-12543
            // So just check 12352-12543
            int kanjiCount = 0;
            foreach (char c in input)
            {
                if (!((int)c > 12352 && (int)c < 12543))
                    kanjiCount++;
            }
            return kanjiCount;
        }

        // Returns true if the pair is comprised of all hiragana characters, like many grammatical words
        static bool allHiragana(string input)
        {
            // Hiragana range 12352-12447
            foreach (char c in input)
            {
                if (!((int)c > 12352 && (int)c < 12447))
                    return false;
            }
            return true;
        }

        // Returns true if the pair is comprised of all katakana characters, used to find loanwords
        static bool allKatakana(string input)
        {
            // Katakana range 12448-12543
            foreach (char c in input)
            {
                if (!((int)c > 12448 && (int)c < 12543))
                    return false;
            }
            return true;
        }

        static void populateBuilder(SortedDictionary<string, string> list, StringBuilder builder, CompareFrequenciesAscending compare, bool sortNonMoraByMoraCount, bool sortBySecondaryPitch, bool showFrequency, int moraScanField)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> listSorted;
            if (sortNonMoraByMoraCount)
                listSorted = list.OrderBy(kvp => moraCount(kvp.Key.Split(" ")[moraScanField])).ThenBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, false)).ThenBy(kvp => kvp, compare);
            else
                listSorted = list.OrderBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, false)).ThenBy(kvp => kvp, compare);
            foreach (var line in listSorted)
            {
                string noFreq = line.Key.Remove(line.Key.LastIndexOf('f') - 1);
                if (showFrequency)
                    builder.Append(line.Key);
                else
                    builder.Append(noFreq + "\n");
            }
        }

        static void populateBuilder(SortedDictionary<string, string> list, StringBuilder builder, CompareFrequenciesAscending compare, bool sortByCharCount, bool sortBySecondaryPitch, bool showFrequency)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> listSorted;
            if (sortByCharCount)
                listSorted = list.OrderBy(kvp => kvp.Key.Split(" ")[0].Length).ThenBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, false)).ThenBy(kvp => kvp, compare);
            else
                listSorted = list.OrderBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, false)).ThenBy(kvp => kvp, compare);
            foreach (var line in listSorted)
            {
                string noFreq = line.Key.Remove(line.Key.LastIndexOf('f') - 1);
                if (showFrequency)
                    builder.Append(line.Key);
                else
                    builder.Append(noFreq + "\n");
            }
        }

        static void populateBuilder(SortedDictionary<string, string> list, StringBuilder builder, CompareFrequenciesAscending compare, bool sortNonMoraByMoraCount, bool sortBySecondaryPitch, bool showFrequency, bool downstepGroup)
        {
            IOrderedEnumerable<KeyValuePair<string, string>> listSorted;
            if (sortNonMoraByMoraCount)
                listSorted = list.OrderBy(kvp => moraCount(kvp.Key.Split(" ")[1])).ThenBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, downstepGroup)).ThenBy(kvp => kvp, compare);
            else
                listSorted = list.OrderBy(kvp => getCompare(kvp.Value, sortBySecondaryPitch, downstepGroup)).ThenBy(kvp => kvp, compare);
            foreach (var line in listSorted)
            {
                string noFreq = line.Key.Remove(line.Key.LastIndexOf('f') - 1);
                if (showFrequency)
                    builder.Append(line.Key);
                else
                    builder.Append(noFreq + "\n");
            }
        }

        // Gets how the comparison is being made for sorting, based on sorting mode
        static string getCompare(string input, bool whole, bool downstepGroup)
        {
            if (downstepGroup)
            {
                HashSet<string> digits = new HashSet<string>() { "2", "3", "4", "5", "6", "7", "8", "9" };
                foreach (string s in digits)
                    input = input.Replace(s, "1");
            }
            if (whole)
                return input;
            else
                return input.First() + "";
        }

        // Simple class used to compare words with the same pitch values by frequency, ascending
        public class CompareFrequenciesAscending : IComparer<KeyValuePair<string, string>>
        {
            public int Compare(KeyValuePair<string, string> first, KeyValuePair<string, string> second)
            {
                // If the pitch values are the same, order by frequency
                int freqStart = first.Key.LastIndexOf('q') + 1;
                string freqString = first.Key.Substring(freqStart);
                int freq1 = Int32.Parse(freqString);

                freqStart = second.Key.LastIndexOf('q') + 1;
                freqString = second.Key.Substring(freqStart);
                int freq2 = Int32.Parse(freqString);

                return freq1 - freq2;
            }
        }
    }
}
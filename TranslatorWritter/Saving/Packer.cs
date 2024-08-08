using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;
using static TranslatorWritter.Packer;

namespace TranslatorWritter {
    public class Packer {        
        const char delimiter='§';
        const string newLine="\n";
        public event EventHandler<SampleEventArgs> ProgressChange;

        public class SampleEventArgs :EventArgs{
            public SampleEventArgs(float percentage) { Percentage = percentage; }
            public float Percentage; // readonly
        }
        
        public void CreatePackageAsync(string[] filePaths, string outputFile, IProgress<float> progress = null) {
            int totalFiles = filePaths.Length*2;
            int currentFile = 0; 
            ComprimeLang[]langs= new ComprimeLang[filePaths.Length];
            // Kontrola delimiter, ve spojovanym sóboro nesmy bet znak '§'!
            foreach (string filePath in filePaths) {        
                using (StreamReader sr = new StreamReader(filePath)) {
                    string content = sr.ReadToEnd();
                    if (content.Contains(delimiter)) throw new InvalidOperationException($"File {filePath} contains the separator '{delimiter}'!");
                    Lang l=new Lang();
                    l.Load(content.Split(new string[] { "\r\n" },StringSplitOptions.RemoveEmptyEntries));

                    langs[currentFile] = new ComprimeLang(l);
                }    
                currentFile++;
                float percentage = (float)currentFile / totalFiles; 
                if (progress != null) {    
                    progress.Report(percentage);
                }
                    
                ProgressChange.Invoke(null, new SampleEventArgs(percentage));
            }

            StringBuilder sb=new StringBuilder();
            sb.Append(FormMain.NewestSaveVersion+'\n');

            // same lines
            if (true) {
                foreach (string s in OptimizeSameString(langs, "SimpleWord")) sb.Append(s+'\n');
                sb.Append("-\n");
                
                foreach (string s in OptimizeSameString(langs, "Phrase")) sb.Append(s+'\n');
                sb.Append("-\n");

                foreach (string s in OptimizeSameString(langs, "Sentence")) sb.Append(s+'\n');
                sb.Append("-\n");

                foreach (string s in OptimizeSameString(langs, "SentencePart")) sb.Append(s+'\n');
                sb.Append("-\n");

                foreach (string s in OptimizeSameString(langs, "SentencePattern")) sb.Append(s+'\n');
                sb.Append("-\n");

                foreach (string s in OptimizeSameString(langs, "SentencePatternPart")) sb.Append(s+'\n');
                sb.Append("-\n"); 

                foreach (string s in OptimizeSameString(langs, "ReplaceS")) sb.Append(s+'\n');
                sb.Append("-\n"); 

                foreach (string s in OptimizeSameString(langs, "ReplaceG")) sb.Append(s+'\n');
                sb.Append("-\n"); 

                foreach (string s in OptimizeSameString(langs, "ReplaceE")) sb.Append(s+'\n');
                sb.Append("-\n"); 
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternNounFrom")) sb.Append(s+'\n');
                sb.Append("-\n");  
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternNounTo")) sb.Append(s+'\n');
                sb.Append("-\n"); 
                        
                foreach (string s in OptimizeSamePatternData(langs, "PatternAdjectiveFrom")) sb.Append(s+'\n');
                sb.Append("-\n");  
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternAdjectiveTo")) sb.Append(s+'\n');
                sb.Append("-\n"); 
                                
                foreach (string s in OptimizeSamePatternData(langs, "PatternPronounFrom")) sb.Append(s+'\n');
                sb.Append("-\n");  
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternPronounTo")) sb.Append(s+'\n');
                sb.Append("-\n");  

                foreach (string s in OptimizeSamePatternData(langs, "PatternNumberFrom")) sb.Append(s+'\n');
                sb.Append("-\n");  
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternNumberTo")) sb.Append(s+'\n');
                sb.Append("-\n");  
                                
                foreach (string s in OptimizeSamePatternData(langs, "PatternVerbFrom")) sb.Append(s+'\n');
                sb.Append("-\n");  
                
                foreach (string s in OptimizeSamePatternData(langs, "PatternVerbTo")) sb.Append(s+'\n');
                sb.Append("-\n");   
                
                foreach (string s in OptimizeSameString(langs, "Adverb")) sb.Append(s+'\n');
                sb.Append("-\n");        
                
                foreach (string s in OptimizeSameString(langs, "Preposition")) sb.Append(s+'\n');
                sb.Append("-\n");     
                
                foreach (string s in OptimizeSameString(langs, "Conjunction")) sb.Append(s+'\n');
                sb.Append("-\n");        
                
                foreach (string s in OptimizeSameString(langs, "Particle")) sb.Append(s+'\n');
                sb.Append("-\n");    
                
                foreach (string s in OptimizeSameString(langs, "Interjection")) sb.Append(s+'\n');
                sb.Append("-\n");
            }

 
            // Zapsat soubor
            using (StreamWriter sw = new StreamWriter(outputFile)) {
                foreach (ComprimeLang l in langs) {
                    List<string> lines=l.GetSavedLinesForPacker();
                    foreach (string s in lines) sb.Append(s+'\n');


                    currentFile++;
                    // Report progress
                    float percentage = (float)currentFile / totalFiles;
                    if (progress != null) {
                        progress.Report(percentage);
                    }
                        
                    ProgressChange.Invoke(null, new SampleEventArgs(percentage));
                    sw.Write(sb);
                    sb.Clear();
                    sw.Write(delimiter);
                }
            }

            Debug.WriteLine("Done package");
           

            /*List<string> linesOfPackage=new List<string>();

            foreach (string filePath in filePaths) {    
                List<string> content = GetFileOptimizedForPackage(filePath);
                    
                linesOfPackage.AddRange(content);

                // Oddělovač
                //sw.Write(delimiter);
                linesOfPackage.Add(delimiter.ToString());

                currentFile++;
                // Report progress
                float percentage = (float)currentFile / totalFiles;
                if (progress != null) {
                    progress.Report(percentage);
                }
                        
                ProgressChange.Invoke(null, new SampleEventArgs(percentage));
            }
            
            string [] linesToSave = OptimizeFile(linesOfPackage);*/
                
          //  File.WriteAllLines(outputFile,linesToSave);
            //using (StreamWriter sw = new StreamWriter(outputFile)) {
            //    sw.Write(string.Join("\n", linesToSave));
            //}

            //List<string> GetFileOptimizedForPackage(string filePath) { 
            //    string[] lines = File.ReadAllLines(filePath);
            //    List<string> newLines=new List<string>();

            //    var _LoadedSaveVersion = FormMain.LoadedSaveVersion;
            //    var _LoadedVersionNumber = FormMain.LoadedVersionNumber;

            //    int i=0;
            //    FormMain.LoadedSaveVersion=lines[0];
            //    if (FormMain.LoadedSaveVersion.Length>4){
            //        string num=FormMain.LoadedSaveVersion.Substring(4);
            //        if (num=="1.0") FormMain.LoadedVersionNumber=1;
            //        else if (num=="0.1") FormMain.LoadedVersionNumber=0;
            //        else {
            //            if (float.TryParse(num, out FormMain.LoadedVersionNumber)) { } else FormMain.LoadedVersionNumber=-1;
            //        }
            //    }
    
            //    List<Source> Cites=new List<Source>();

            //    // head
            //    for (i++; i<lines.Length; i++) { 
            //        string line=lines[i];
            //        if (line=="-") break;

            //        if (line.StartsWith("d")) continue;
            //        if (line.StartsWith("l")) continue;
            //        if (line.StartsWith("x")) continue;
            //        if (line.StartsWith("z")) continue;
            //        if (line.StartsWith("f")) continue;
            //      //  if (line.StartsWith("t")) continue;
            //        if (line.StartsWith("a")) continue;
            //        if (line.StartsWith("i")) continue;

            //        if (line.StartsWith("t")) {newLines.Add(line); continue;}
            //        if (line.StartsWith("c")) {newLines.Add(line); continue;}

            //        // Cites
            //        if (line.StartsWith("b")) {
            //            string[] lines_cites=line.Substring(1).Split(new string[] { "\\n" }, StringSplitOptions.None);
            //            foreach (string line_cite in lines_cites) {                             
            //                Source s= new Source();
            //                if (s.Load(line_cite)) { 
            //                    Cites.Add(s);
            //                }
            //            }
                        
            //            Source.BuildSourcesIds(Cites);

            //            newLines.Add(/*line*/Source.SavePacker(Cites)); continue;
            //        }
            //        if (line.StartsWith("o")) {newLines.Add(line); continue;}
            //        if (line.StartsWith("q")) {newLines.Add(line); continue;}
            //        if (line.StartsWith("u")) {newLines.Add(line); continue;}
            //        if (line.StartsWith("r")) {newLines.Add(line); continue;}
            //        if (line.StartsWith("g")){
            //            if (line.Length>3 && line.Contains(',')){
            //                string[]pos=line.Substring(1).Split(',');
            //                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //                ci.NumberFormat.CurrencyDecimalSeparator = ".";

            //                float x=float.Parse(pos[0], NumberStyles.Any, ci);

            //                float y=float.Parse(pos[1], NumberStyles.Any, ci);
            //              //  if (isX && isY){
            //                    x=(float)Math.Round(x,5);
            //                    y=(float)Math.Round(y,5);
            //                    newLines.Add("g"+x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+y.ToString(System.Globalization.CultureInfo.InvariantCulture)); 
            //                    continue;
            //              //  }
            //            }
            //        }
            //    }
            //    newLines.Add("-");                              
                
            //    // SentencePattern
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemSentencePattern s = ItemSentencePattern.Load(line); 
            //        if (s==null) continue;
            //        if (s.From=="") continue;
            //        lang.SentencePattern.Add(s);
            //        newLines.Add(s.Save());
            //    }
            //    newLines.Add("-");

            //    // SentencePatternPart
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemSentencePatternPart s = ItemSentencePatternPart.Load(line);
            //        if (s==null) continue;
            //        if (s.From=="") continue;

            //        newLines.Add(s.Save());
            //    }
            //    newLines.Add("-");

            //    // Sentences
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemSentence s = ItemSentence.Load(line);
            //        if (s==null) continue;
            //        if (s.From=="") continue;

            //        string saveP=s.SavePacker(Cites);
            //        if (saveP!=null)newLines.Add(saveP);
            //    }
            //    newLines.Add("-");

            //    // SentencePart
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemSentencePart s = ItemSentencePart.Load(line);
            //        if (s==null) continue;
            //        if (s.From=="") continue;

            //        string saveP=s.SavePacker(Cites);
            //        if (saveP!=null)newLines.Add(saveP);
            //    }
            //    newLines.Add("-");

            //    // Phrase
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPhrase s = ItemPhrase.Load(line);    
            //        if (s==null) continue;
            //        char[] notAllowed=new char[]{'?', ';', '\t'};
            //        if (Methods.Contains(s.From, notAllowed)) continue;
            //        if (Methods.Contains(s.To, notAllowed)) continue;
            //        string saveP=s.SavePacker(Cites);
            //        if (saveP!=null)newLines.Add(saveP);
            //    }
            //    newLines.Add("-");

            //    // SimpleWords
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemSimpleWord s = ItemSimpleWord.Load(line);                
            //        if (s==null) continue;

            //        char[] notAllowed=new char[]{'?', ' ', ';', '\t'};
            //        if (s.From.Contains(notAllowed)) continue;
            //        if (Methods.Contains(s.To, notAllowed)) continue;

            //        string saveP=s.SavePacker(Cites);
            //        if (saveP!=null)newLines.Add(saveP);
            //    }
            //    newLines.Add("-");

            //    // ReplaceS
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemReplaceS s = ItemReplaceS.Load(line);

            //        char[] notAllowed=new char[]{'?', ' ', ';', ',', '\t'};
            //        if (s.From.Contains(notAllowed)) continue;
            //        if (s.To.Contains(notAllowed)) continue;


            //        if (s==null) continue;

            //        newLines.Add(s.Save());
            //    }
            //    newLines.Add("-");

            //    // ReplaceG
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemReplaceG s = ItemReplaceG.Load(line);
            //        if (s==null) continue;

            //        newLines.Add(s.Save());
            //    }
            //    newLines.Add("-");

            //    // ReplaceE
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemReplaceE s = ItemReplaceE.Load(line);
            //        if (s==null) continue;

            //        newLines.Add(s.Save());
            //    }
            //    newLines.Add("-");

            //    List<ItemPatternNoun> listPatternFromNoun=new List<ItemPatternNoun>(), listPatternToNoun=new List<ItemPatternNoun>();

            //    // PatternNounFrom
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternNoun s = ItemPatternNoun.Load(line);
            //        if (s==null) continue;
            //        if (!s.IsEmpty()) listPatternFromNoun.Add(s);                    
            //        //newLines.Add(s.SavePacker());
            //    }
            //    // optimize names
            //    List<PairTranslating> pairsNounFrom = Methods.OptimizeNamesToPacker(listPatternFromNoun.ConvertAll(p => p as ItemTranslatingPattern));                
            //    // remove same ones
            //    RelocateSame(pairsNounFrom);
            //    // save
            //    foreach (PairTranslating patternNounFrom in pairsNounFrom) { 
            //        if (!patternNounFrom.IsDuplicate) newLines.Add(((ItemPatternNoun)patternNounFrom.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // PatternNounTo
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternNoun s = ItemPatternNoun.Load(line);
            //        if (s.IsEmpty()) continue;
            //        if (s==null) continue;
                    
            //        if (!s.IsEmpty()) listPatternToNoun.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsNounTo = Methods.OptimizeNamesToPacker(listPatternToNoun.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsNounTo);
            //    foreach (PairTranslating patternNounTo in pairsNounTo) { 
            //        if (!patternNounTo.IsDuplicate)  newLines.Add(((ItemPatternNoun)patternNounTo.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // Noun
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemNoun s = ItemNoun.Load(line);
            //        if (s==null) continue;
            //        if (s.From.Contains("?")) continue;
            //        if (s.From.Contains(" ")) continue;
            //        if (s.From.Contains(",")) continue;
            //        //if (s.From=="") continue;
            //       // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

            //        /* backup
            //        bool pf=false;
            //        foreach (ItemPatternNoun f in listPatternFromNoun) { 
            //            if (f.Name == s.PatternFrom) { 
            //                pf=true;
            //                break;
            //            }
            //        }
            //        if (!pf) continue;

            //        bool pt = false;
            //        foreach (ItemPatternNoun t in listPatternToNoun) {
            //            foreach (var to in s.To) {
            //                if (t.Name == to.Pattern) {
            //                    pt = true;
            //                    break;
            //                }
            //            }
            //            if (pt) break;
            //        }*/

            //        // convert names simplify (string name to "id")
            //        // From
            //        bool pf=false;
            //        ItemPatternNoun f = (ItemPatternNoun)Methods.GetOptimizedNameForPacker(pairsNounFrom, s.PatternFrom);
            //        if (f!=null) { 
            //            pf=true;
            //            s.PatternFrom=f.Name;
            //        }                    
            //        if (!pf) continue;

            //        // To
            //        bool pt = false;
            //        for (int to_i=0; to_i<s.To.Count; to_i++) {
            //            var to = s.To[to_i];
            //            bool existsInside=false;
            //            foreach (PairTranslating patternPair in pairsNounTo) { 
            //                if (to.Pattern==patternPair.NonSimplifiedName) { 
            //                    to.Pattern=patternPair.Pattern.Name;
            //                    existsInside=true;
            //                    pt=true;
            //                    break;
            //                }
            //            }
            //            if (!existsInside) { 
            //                s.To.Remove(to);
            //                to_i--;
            //            }
            //        }                    
            //        if (!pt) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    List<ItemPatternAdjective> listPatternFromAdjective=new List<ItemPatternAdjective>(), listPatternToAdjective=new List<ItemPatternAdjective>();

            //    // PatternAdjectives
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternAdjective s = ItemPatternAdjective.Load(line);
            //        if (s==null) continue;
                   
            //        if (!s.IsEmpty())  listPatternFromAdjective.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsAdjectiveFrom = Methods.OptimizeNamesToPacker(listPatternFromAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsAdjectiveFrom);
            //    foreach (PairTranslating patternAdjectiveFrom in pairsAdjectiveFrom) { 
            //        if (!patternAdjectiveFrom.IsDuplicate) newLines.Add(((ItemPatternAdjective)patternAdjectiveFrom.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // PatternAdjectivesTo
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternAdjective s = ItemPatternAdjective.Load(line);
            //        if (s==null) continue;                    

            //        if (!s.IsEmpty()) listPatternToAdjective.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsAdjectiveTo = Methods.OptimizeNamesToPacker(listPatternToAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsAdjectiveTo);
            //    foreach (PairTranslating patternAdjectiveTo in pairsAdjectiveTo) { 
            //        if (!patternAdjectiveTo.IsDuplicate) newLines.Add(((ItemPatternAdjective)patternAdjectiveTo.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // Adjectives
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "")continue;
            //        ItemAdjective s = ItemAdjective.Load(line);
            //        if (s==null) continue;
            //      //  if (s.From=="") continue;
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

            //        /*bool pf=false;
            //        foreach (ItemPatternAdjective f in listPatternFromAdjective) { 
            //            if (f.Name == s.PatternFrom) { 
            //                pf=true;
            //                break;
            //            }
            //        }
            //        if (!pf) continue;

            //        bool pt = false;
            //        foreach (ItemPatternAdjective t in listPatternToAdjective) {
            //            foreach (var to in s.To){
            //                if (t.Name == to.Pattern) {
            //                    pt = true;
            //                    break;
            //                }
            //            }
            //            if (pt) break;
            //        }
            //        if (!pt) continue;*/

            //        // convert names simplify (string name to "id")
            //        // From
            //        bool pf=false;
            //        ItemPatternAdjective f = (ItemPatternAdjective)Methods.GetOptimizedNameForPacker(pairsAdjectiveFrom, s.PatternFrom);
            //        if (f!=null) { 
            //            pf=true;
            //            s.PatternFrom=f.Name;
            //        }                    
            //        if (!pf) continue;

            //        // To
            //        bool pt = false;
            //        for (int to_i=0; to_i<s.To.Count; to_i++) {
            //            var to = s.To[to_i];
            //            bool existsInside=false;
            //            foreach (PairTranslating patternPair in pairsAdjectiveTo) { 
            //                if (to.Pattern==patternPair.NonSimplifiedName) { 
            //                    to.Pattern=patternPair.Pattern.Name;
            //                    existsInside=true;
            //                    pt=true;
            //                    break;
            //                }
            //            }
            //            if (!existsInside) { 
            //                s.To.Remove(to);
            //                to_i--;
            //            }
            //        }                    
            //        if (!pt) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    List<ItemPatternPronoun> listPatternFromPronoun=new List<ItemPatternPronoun>(), listPatternToPronoun=new List<ItemPatternPronoun>();

            //    // PatternPronounsFrom
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternPronoun s = ItemPatternPronoun.Load(line);
            //        if (s==null) continue;
                    
            //        if (!s.IsEmpty()) listPatternFromPronoun.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsPronounFrom = Methods.OptimizeNamesToPacker(listPatternFromPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsPronounFrom);
            //    foreach (PairTranslating patternPronounFrom in pairsPronounFrom) { 
            //        if (!patternPronounFrom.IsDuplicate) newLines.Add(((ItemPatternPronoun)patternPronounFrom.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // PatternPronounsTo
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternPronoun s = ItemPatternPronoun.Load(line);
            //        if (s==null) continue;
                    
            //        if (!s.IsEmpty()) listPatternToPronoun.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsPronounTo = Methods.OptimizeNamesToPacker(listPatternToPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsPronounTo);
            //    foreach (PairTranslating patternPronounTo in pairsPronounTo) { 
            //        if (!patternPronounTo.IsDuplicate) newLines.Add(((ItemPatternPronoun)patternPronounTo.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // Pronouns
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "")  continue;
            //        ItemPronoun s = ItemPronoun.Load(line);
            //        if (s==null) continue;
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

            //        /*bool pf=false;
            //        foreach (ItemPatternPronoun f in listPatternFromPronoun) { 
            //            if (f.Name == s.PatternFrom) { 
            //                pf=true;
            //                break;
            //            }
            //        }
            //        if (!pf) continue;

            //        bool pt = false;
            //        foreach (ItemPatternPronoun t in listPatternToPronoun) {
            //            foreach (var to in s.To){
            //                if (t.Name == to.Pattern) {
            //                    pt = true;
            //                    break;
            //                }
            //            }
            //            if (pt) break;
            //        }
            //        if (!pt) continue;*/
            //        // convert names simplify (string name to "id")
            //        // From
            //        bool pf=false;
            //        ItemPatternPronoun f = (ItemPatternPronoun)Methods.GetOptimizedNameForPacker(pairsPronounFrom, s.PatternFrom);
            //        if (f!=null) { 
            //            pf=true;
            //            s.PatternFrom=f.Name;
            //        }                    
            //        if (!pf) continue;

            //        // To
            //        bool pt = false;
            //        for (int to_i=0; to_i<s.To.Count; to_i++) {
            //            var to = s.To[to_i];
            //            bool existsInside=false;
            //            foreach (PairTranslating patternPair in pairsPronounTo) { 
            //                if (to.Pattern==patternPair.NonSimplifiedName) { 
            //                    to.Pattern=patternPair.Pattern.Name;
            //                    existsInside=true;
            //                    pt=true;
            //                    break;
            //                }
            //            }
            //            if (!existsInside) { 
            //                s.To.Remove(to);
            //                to_i--;
            //            }
            //        }
            //        if (!pt) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    List<ItemPatternNumber> listPatternFromNumber=new List<ItemPatternNumber>(), listPatternToNumber=new List<ItemPatternNumber>();

            //    // PatternNumbersFrom
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "")  continue;
            //        ItemPatternNumber s = ItemPatternNumber.Load(line);
            //        if (s==null) continue;
                    
            //        if (!s.IsEmpty()) listPatternFromNumber.Add(s);
            //           // newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsNumberFrom = Methods.OptimizeNamesToPacker(listPatternFromNumber.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsNumberFrom);
            //    foreach (PairTranslating patternNumberFrom in pairsNumberFrom) { 
            //        if (!patternNumberFrom.IsDuplicate) newLines.Add(((ItemPatternNumber)patternNumberFrom.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // PatternNumbersTo
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternNumber s = ItemPatternNumber.Load(line);
            //        if (s==null) continue;
                    
            //        if (!s.IsEmpty()) listPatternToNumber.Add(s);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsNumberTo = Methods.OptimizeNamesToPacker(listPatternToNumber.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsNounTo);
            //    foreach (PairTranslating patternNumberTo in pairsNumberTo) { 
            //        if (!patternNumberTo.IsDuplicate) newLines.Add(((ItemPatternNumber)patternNumberTo.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // Numbers
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemNumber s = ItemNumber.Load(line);
            //        if (s==null) continue;
            //      //  if (s.From=="") continue;
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

            //        /*bool pf=false;
            //        foreach (ItemPatternNumber f in listPatternFromNumber) { 
            //            if (f.Name == s.PatternFrom) { 
            //                pf=true;
            //                break;
            //            }
            //        }
            //        if (!pf) continue;
            //        bool pt = false;
            //        foreach (ItemPatternNumber t in listPatternToNumber) {
            //            foreach (var to in s.To) {
            //                if (t.Name == to.Pattern) {
            //                    pt = true;
            //                    break;
            //                }
            //            }
            //            if (pt) break;
            //        }
            //        if (!pt) continue;*/
            //        // convert names simplify (string name to "id")
            //        // From
            //        bool pf=false;
            //        ItemPatternNumber f = (ItemPatternNumber)Methods.GetOptimizedNameForPacker(pairsNumberFrom, s.PatternFrom);
            //        if (f!=null) { 
            //            pf=true;
            //            s.PatternFrom=f.Name;
            //        }                    
            //        if (!pf) continue;

            //        // To
            //        bool pt = false;
            //        for (int to_i=0; to_i<s.To.Count; to_i++) {
            //            var to = s.To[to_i];
            //            bool existsInside=false;
            //            foreach (PairTranslating patternPair in pairsNumberTo) { 
            //                if (to.Pattern==patternPair.NonSimplifiedName) { 
            //                    to.Pattern=patternPair.Pattern.Name;
            //                    existsInside=true;
            //                    pt=true;
            //                    break;
            //                }
            //            }
            //            if (!existsInside) { 
            //                s.To.Remove(to);
            //                to_i--;
            //            }
            //        }
            //        if (!pt) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    List<ItemPatternVerb> listPatternFromVerb=new List<ItemPatternVerb>(), listPatternToVerb=new List<ItemPatternVerb>();

            //    // PatternVerbsFrom
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternVerb s = ItemPatternVerb.Load(line);
            //        if (s==null) continue;
            //        if (s.Name=="") continue; 
            //      //  s.Infinitive=Methods.SavePackerStringMultiple()
                  
            //        if (!s.IsEmpty()) listPatternFromVerb.Add(s);  //newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsVerbFrom = Methods.OptimizeNamesToPacker(listPatternFromVerb.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsVerbFrom);
            //    foreach (PairTranslating patternVerbFrom in pairsVerbFrom) { 
            //        if (!patternVerbFrom.IsDuplicate) newLines.Add(((ItemPatternVerb)patternVerbFrom.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // PatternVerbsTo
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPatternVerb s = ItemPatternVerb.Load(line);
            //        if (s==null) continue;
            //        if (s.Name=="") continue; 
                    
            //        if (!s.IsEmpty()) listPatternToVerb.Add(s/*ItemPatternVerb.Load(line)*/);//newLines.Add(s.SavePacker());
            //    }
            //    List<PairTranslating> pairsVerbTo = Methods.OptimizeNamesToPacker(listPatternToVerb.ConvertAll(p => p as ItemTranslatingPattern));
            //    // remove same ones
            //    RelocateSame(pairsVerbTo);
            //    foreach (PairTranslating patternVerbTo in pairsVerbTo) { 
            //        if (!patternVerbTo.IsDuplicate) newLines.Add(((ItemPatternVerb)patternVerbTo.Pattern).SavePacker());
            //    }
            //    newLines.Add("-");

            //    // Verb
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemVerb s = ItemVerb.Load(line);
            //        if (s==null) continue;
            //        char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (s.From=="") continue;
            //        if (s.From.Contains(notAllowed)) continue;
            //        // if (Methods.ContainsBody(s.To, notAllowed)) continue;

            //        /*bool pf = false;
            //        foreach (ItemPatternVerb f in listPatternFromVerb) {
            //            if (f.Name == s.PatternFrom) {
            //                pf = true;
            //                break;
            //            }
            //        }
            //        if (!pf) continue;

            //        bool pt = false;
            //        foreach (ItemPatternVerb t in listPatternToVerb) {
            //            foreach (var to in s.To){
            //                if (t.Name == to.Pattern) {
            //                    pt = true;
            //                    break;
            //                }
            //            }
            //            if (pt) break;
            //        }
            //        if (!pt) continue;*/
            //        // convert names simplify (string name to "id")
            //        // From
            //        bool pf=false;
            //        ItemPatternVerb f = (ItemPatternVerb)Methods.GetOptimizedNameForPacker(pairsVerbFrom, s.PatternFrom);
            //        if (f!=null) { 
            //            pf=true;
            //            s.PatternFrom=f.Name;
            //        }                    
            //        if (!pf) continue;

            //        // To
            //        bool pt = false;
            //        for (int to_i=0; to_i<s.To.Count; to_i++) {
            //            var to = s.To[to_i];
            //            bool existsInside=false;
            //            foreach (PairTranslating patternPair in pairsVerbTo) {
            //                if (to.Pattern==patternPair.NonSimplifiedName) { 
            //                    to.Pattern=patternPair.Pattern.Name;
            //                    existsInside=true;
            //                    pt=true;
            //                    break;
            //                }
            //            }
            //            if (!existsInside) { 
            //                s.To.Remove(to);
            //                to_i--;
            //            }
            //        }
            //        if (!pt) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    // Adverb
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemAdverb s = ItemAdverb.Load(line);
            //        if (s==null) continue;
            //       // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //        if (s.From=="") continue;
            //       // if (s.From.Contains(notAllowed)) continue;
            //       // if (Methods.Contains(s.To, notAllowed)) continue;
                    
            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    // Preposition
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "")  continue;
            //        ItemPreposition s = ItemPreposition.Load(line);
            //        if (s==null) continue;
            //        if (s.From=="") continue;
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //      //  if (s.From.Contains(notAllowed)) continue;
            //       // if (Methods.Contains(s.To, notAllowed)) continue;
            //        string data=s.SavePacker(Cites);
            //        if (data!=null) newLines.Add(data);
            //    }
            //    newLines.Add("-");

            //    // Conjunction
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-")  break;
            //        if (line == "")  continue;
            //        ItemConjunction s = ItemConjunction.Load(line);
            //        if (s==null) continue;
            //        if (s.From=="") continue;
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //       // if (s.From.Contains(notAllowed)) continue;
            //        //if (Methods.Contains(s.To, notAllowed)) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    // Particle
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-")  break;
            //        if (line == "")  continue;
            //        ItemParticle s = ItemParticle.Load(line);
            //       // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //       // if (s.From.Contains(notAllowed)) continue;
            //        if (s.From=="") continue;
            //        //if (Methods.Contains(s.To, notAllowed)) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    // Interjection
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "")  continue;
            //        ItemInterjection s = ItemInterjection.Load(line);
            //      //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
            //        if (s.From=="") continue;
            //       // if (s.From.Contains(notAllowed)) continue;
            //        //if (Methods.Contains(s.To, notAllowed)) continue;

            //        newLines.Add(s.SavePacker(Cites));
            //    }
            //    newLines.Add("-");

            //    // PhrasePattern
            //    for (i++; i < lines.Length; i++) {
            //        string line = lines[i];
            //        if (line == "-") break;
            //        if (line == "") continue;
            //        ItemPhrasePattern s = ItemPhrasePattern.Load(line);
            //        if (s.From=="") continue;
            //        newLines.Add(s.Save());
            //    }
            // //  newLines.Add("-");

                
            //    FormMain.LoadedSaveVersion = _LoadedSaveVersion;
            //    FormMain.LoadedVersionNumber = _LoadedVersionNumber;

            //    return newLines;
            //}
        } 

        //public void CreatePackageAsync(string[] filePaths, string outputFile, IProgress<float> progress = null) {
        //    int totalFiles = filePaths.Length*2;
        //    int currentFile = 0; 
            
        //    // Kontrola delimiter, ve spojovanym sóboro nesmy bet znak '§'!
        //    foreach (string filePath in filePaths) {        
        //        using (StreamReader sr = new StreamReader(filePath)) {
        //            string content = sr.ReadToEnd();
        //            if (content.Contains(delimiter)) throw new InvalidOperationException($"File {filePath} contains the separator '{delimiter}'!");
        //        }    
        //        currentFile++;
        //        float percentage = (float)currentFile / totalFiles; 
        //        if (progress != null) {    
        //            progress.Report(percentage);
        //        }
        //            ProgressChange.Invoke(null, new SampleEventArgs(percentage));
        //    }
 
        //    // Zapsat soubor
        //    using (StreamWriter sw = new StreamWriter(outputFile)) {
        //        foreach (string filePath in filePaths) {
        //            // Název souboru
        //            sw.Write(Path.GetFileName(filePath));

        //            sw.Write(delimiter);

        //            // Zapsat soubor
        //            using (StreamReader sr = new StreamReader(filePath)) {
        //                sw.Write(sr.ReadToEnd());
        //            }

        //            // Oddělovač
        //            sw.Write(delimiter);

        //            currentFile++;
        //            // Report progress
        //                float percentage = (float)currentFile / totalFiles;
        //            if (progress != null) {
        //                progress.Report(percentage);
        //            }
                        
        //            ProgressChange.Invoke(null, new SampleEventArgs(percentage));
        //        }
        //    }

        ////    Done.Invoke(this, null);
        //}   

        public static void ExtractMergedFiles(string inputFile, string outputFolder) {
            string fileContent = File.ReadAllText(inputFile);
            string[] fileContents = fileContent.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            // Po souborech
            for (int i = 0; i < fileContents.Length; i += 2) {
                string fileName = fileContents[i], 
                    fileText = fileContents[i + 1];

                string filePath = Path.Combine(outputFolder, fileName);

                // Zápis souboru
                using (StreamWriter sw = new StreamWriter(filePath)) sw.Write(fileText);
            }
        }

        void RelocateSame(List<PairTranslating> pairsNoun) {
            // From
            for (int i=0; i<pairsNoun.Count; i++) {
                PairTranslating patternNoun = pairsNoun[i];
                ItemTranslatingPattern pattern=(ItemTranslatingPattern)patternNoun.Pattern;
                    
                // compare to...
                for (int j=i+1; j<pairsNoun.Count; j++) {
                    PairTranslating patternNoun2 = pairsNoun[j];
                    ItemTranslatingPattern pattern2=(ItemTranslatingPattern)patternNoun2.Pattern;

                    if (pattern==pattern2) throw new Exception("that's strange... i+1");

                    if (pattern.IsSameAs(pattern2)) {
                        patternNoun2.IsDuplicate=true;
                        //patternNoun2.RelocateUnderSimplifiedName=patternNoun.Pattern.Name;
                        patternNoun2.Pattern.Name=patternNoun.Pattern.Name;
                        break; 
                    }
                }
            }
        }

        static string[] OptimizeSameString(ComprimeLang[] cLangs, string name) {
            List<SameLine> sameLines = new List<SameLine>();
            
            // add samelines
            for (int i=0; i<cLangs.Length; i++) {
                ComprimeLang line=cLangs[i];
                List<SavedStringData> param=(List<SavedStringData>)line[name];
                
                foreach (SavedStringData d in param) { 
                    // if exists in line in sameLines
                    SameLine s=GetSameLine(d.Data);
                    if (s!=null) { 
                        s.Count++;
                        d.Shortcut=s;
                    
                    // else if not exists
                    } else {
                        SameLine nSL=new SameLine{ Count=1, Data=d.Data };
                        d.Shortcut=nSL;
                        sameLines.Add(nSL);    
                    }
                }
            }

            // clear one time lines
            for (int i=0; i<cLangs.Length; i++) {
                ComprimeLang line=cLangs[i];
                List<SavedStringData> param=(List<SavedStringData>)line[name];
                
                foreach (SavedStringData d in param) {
                    if (d.Shortcut.Count==1) d.Shortcut=null;
                }
            }

         
            for (int i=0; i<sameLines.Count; i++) {
                SameLine line = sameLines [i];
                if (line.Count==1) {
                    sameLines.RemoveAt(i);
                    i--;
                }
            }

            // sort from most
            sameLines=sameLines.OrderByDescending(i => i.Count).ToList();

            // set id
          /*  int id_samelines=0;
            foreach (SameLine s in sameLines) { 
                s.Id=id_samelines;
                id_samelines++;
            }*/
             
            string[] strings=new string[sameLines.Count];
            for (int i=0; i<sameLines.Count; i++) {
                sameLines[i].Id=i;
                strings[i]=sameLines[i].saveData();
            }

            return strings;

            SameLine GetSameLine(string line) { 
                foreach (SameLine sameLine in sameLines) { 
                    if (sameLine.Data==line) {
                        return sameLine;    
                    }
                }
                return null;
            }
        }   

        static string[] OptimizeSamePatternData(ComprimeLang[] cLangs, string name) {
            List<SameLine> sameLines = new List<SameLine>();

            // add samelines
            for (int i=0; i<cLangs.Length; i++) {
                ComprimeLang line=cLangs[i];
                List<SavedPatternData> param=(List<SavedPatternData>)line[name];
               
                foreach (SavedPatternData d in param) { 
                    // if exists in line in sameLines
                    SameLine s=GetSameLine(d.PatternData);
                    if (s!=null) { 
                        s.Count++;
                        d.Shortcut=s;
                    
                    // else if not exists
                    } else {
                        SameLine nSL=new SameLine{ Count=1, Data=d.PatternData };
                        d.Shortcut=nSL;
                        sameLines.Add(nSL);    
                    }
                }
            }

            // clear one time lines
            for (int i=0; i<cLangs.Length; i++) {
                ComprimeLang line=cLangs[i];
                List<SavedPatternData> param=(List<SavedPatternData>)line[name];
                
                foreach (SavedPatternData d in param) {
                    if (d.Shortcut.Count==1) d.Shortcut=null;
                }
            }

            for (int i=0; i<sameLines.Count; i++) {
                SameLine line = sameLines[i];
                if (line.Count==1) {
                    sameLines.Remove(line);
                    i--;
                }
            }

            // sort from most
            sameLines=sameLines.OrderByDescending(i => i.Count).ToList();
            
            // set id
           /* int id_samelines=0;
            foreach (SameLine s in sameLines) { 
                s.Id=id_samelines;
                id_samelines++;
            }*/
            
            //return sameLines;
            string[] strings=new string[sameLines.Count];
            for (int i=0; i<sameLines.Count; i++) { 
                sameLines[i].Id=i;
                strings[i]=sameLines[i].saveData();
            }

            return strings;

            SameLine GetSameLine(string line) { 
                foreach (SameLine sameLine in sameLines) { 
                    if (sameLine.Data==line) {
                        return sameLine;    
                    }
                }
                return null;
            }
        }   

        //static string[] OptimizeFile(List<ComprimeLang> cLang) {
        //    // create LineNumbers
        //    LineNumber[] lines=new LineNumber[rawLines.Count];
        //    List<SameLine> sameLines = new List<SameLine>();

        //    // add samelines
        //    for (int i=0; i<cLang.Count; i++) {
        //        string line=rawLines[i];
        //        // if exists in line in sameLines
        //        SameLine s=GetSameLine(line);
        //        if (s!=null) { 
        //            s.Count++;
        //            lines[i]=new LineNumber{Line=line, Number=s};
        //        }
        //        // else if not exists
        //        else lines[i]=new LineNumber{Line=line, Number=new SameLine{ Count=1, Data=line}};
        //    }

        //    // clear one time lines
        //    foreach (LineNumber line in lines) { 
        //        if (line.Number.Count==1) {
        //            line.Number=null;
        //        }
        //    }
        //    foreach (SameLine line in sameLines) { 
        //        if (line.Count==1) sameLines.Remove(line);
        //    }

        //    // sort from most
        //    sameLines=sameLines.OrderBy(i => i.Count).ToList();

        //    string[] outLines = new string[sameLines.Count+1+rawLines.Count];
        //    for (int i=0; i<sameLines.Count; i++) { 
        //        outLines[i]=sameLines[i].saveData();
        //    }
        //    outLines[sameLines.Count]="!DUP END!";

        //    int line_pos=sameLines.Count+1;
        //    foreach (LineNumber line in lines) {
        //        if (line.Number!=null) outLines[line_pos]=line.Number.getShortcut();
        //        else outLines[line_pos]=line.Line;
        //        line_pos++;
        //    }

        //    return outLines;

        //    SameLine GetSameLine(string line) { 
        //        foreach (SameLine sameLine in sameLines) { 
        //            if (sameLine.Data==line) {
        //                return sameLine;    
        //            }
        //        }
        //        return null;
        //    }
        //}       
    }

    public class PairTranslating {
        public string NonSimplifiedName;
        public ItemTranslatingPattern Pattern;

        // for duplicates
        public bool IsDuplicate;
        public string RelocateUnderSimplifiedName;

        public static void RelocateSame(List<PairTranslating> pairsNoun) {
            // From
            for (int i=0; i<pairsNoun.Count; i++) {
                PairTranslating patternNoun = pairsNoun[i];
                ItemTranslatingPattern pattern=(ItemTranslatingPattern)patternNoun.Pattern;
                    
                // compare to...
                for (int j=i+1; j<pairsNoun.Count; j++) {
                    PairTranslating patternNoun2 = pairsNoun[j];
                    ItemTranslatingPattern pattern2=(ItemTranslatingPattern)patternNoun2.Pattern;

                    if (pattern==pattern2) throw new Exception("that's strange... i+1");

                    if (pattern.IsSameAs(pattern2)) {
                        patternNoun2.IsDuplicate=true;
                        //patternNoun2.RelocateUnderSimplifiedName=patternNoun.Pattern.Name;
                        patternNoun2.Pattern.Name=patternNoun.Pattern.Name;
                        break; 
                    }
                }
            }
        }
    }

    public class SameLine{ 
        public int Id;
        public string Data;
        public int Count;

        public string getShortcut() {
            string num=Methods.NumbToBytes(Id);
            //if (num.Length>1) 
                //Debug.WriteLine(num);
            return "!"+num;
        }
        public string saveData() => /*Methods.NumbToBytes(Id)+"|"+*/Data;
    } /**/

   /* public class LineNumber{ 
        public SameLine Number;
        public string Line;
    }*/
}

      /*  public static void CreatePackage(string[] filePaths, string outputFile) { 
            using (StreamWriter sw = new StreamWriter(outputFile)) {
                foreach (string filePath in filePaths) {
                    // Název souboru
                    sw.Write(Path.GetFileName(filePath));

                    // Oddělovač
                    sw.Write(delimiter);

                    // Zapsat soubor
                    using (StreamReader sr = new StreamReader(filePath)) sw.Write(sr.ReadToEnd());

                    // Oddělovač
                    sw.Write(delimiter); 
                }
            }
        }*/
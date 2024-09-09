using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using TranslatorWritter;

namespace TranslatorWritter{
    internal class Lang {
        public List<ItemSimpleWord> itemsSimpleWord=new List<ItemSimpleWord>();
        public List<ItemPhrase> itemsPhrase=new List<ItemPhrase>();
        public List<ItemSentencePart> itemsSentencePart=new List<ItemSentencePart>();
        public List<ItemSentence> itemsSentence=new List<ItemSentence>();
        public List<ItemSentencePatternPart> itemsSentencePatternPart=new List<ItemSentencePatternPart>();
        public List<ItemSentencePattern> itemsSentencePattern=new List<ItemSentencePattern>();
        public List<ItemReplaceS> itemsReplaceS=new List<ItemReplaceS>();
        public List<ItemReplaceG> itemsReplaceG=new List<ItemReplaceG>();
        public List<ItemReplaceE> itemsReplaceE=new List<ItemReplaceE>();
        public List<ItemConjunction> itemsConjunction=new List<ItemConjunction>();
        public List<ItemParticle> itemsParticle=new List<ItemParticle>();
        public List<ItemInterjection> itemsInterjection=new List<ItemInterjection>();
        public List<ItemPhrasePattern> itemsPhrasePattern=new List<ItemPhrasePattern>();
        public List<ItemAdverb> itemsAdverb=new List<ItemAdverb>();

        public List<ItemPatternNoun> itemsPatternNounFrom=new List<ItemPatternNoun>(), itemsPatternNounTo=new List<ItemPatternNoun>();

        public List<ItemNoun> itemsNoun=new List<ItemNoun>();
        public List<ItemPatternPronoun> itemsPatternPronounFrom=new List<ItemPatternPronoun>(), itemsPatternPronounTo=new List<ItemPatternPronoun>();
        public List<ItemPronoun> itemsPronoun=new List<ItemPronoun>();
        public List<ItemPatternAdjective> itemsPatternAdjectiveFrom=new List<ItemPatternAdjective>(), itemsPatternAdjectiveTo=new List<ItemPatternAdjective>();
        public List<ItemAdjective> itemsAdjective=new List<ItemAdjective>();
        public List<ItemPatternVerb> itemsPatternVerbFrom=new List<ItemPatternVerb>(), itemsPatternVerbTo=new List<ItemPatternVerb>();
        public List<ItemVerb> itemsVerb=new List<ItemVerb>();
        public List<ItemPatternNumber> itemsPatternNumberFrom=new List<ItemPatternNumber>(), itemsPatternNumberTo=new List<ItemPatternNumber>();
        public List<ItemNumber> itemsNumber=new List<ItemNumber>();
        public List<ItemPreposition> itemsPreposition=new List<ItemPreposition>();

        public List<Source> Cites;

     //   string LoadedSaveVersion;
       // float LoadedVersionNumber;
        public string Info;
        
        public int Country;
        public int TypeLang;
        public int Quality;
        public string GPS;
        public string SpadaPod;
        public string Oblast;
        public string Comment;
        public string Original;
        public string Zachytne;
        public string Author;
        public string LastDate;
        public string Cite;
        public string LangL;
        public string LangLocation;
        public string LangFrom;
        public string Select;


        public void Load(string[] lines) { 
           // List<string> newLines=new List<string>();
            int i=0;
            string LoadedSaveVersion=lines[i];
            
            string _LoadedSaveVersion=FormMain.LoadedSaveVersion;
            float _LoadedVersionNumber=FormMain.LoadedVersionNumber;
            FormMain.LoadedSaveVersion=LoadedSaveVersion;
            if (LoadedSaveVersion.Length>4){
                string num=LoadedSaveVersion.Substring(4);
                if (num=="1.0") FormMain.LoadedVersionNumber=1;
                else if (num=="0.1") FormMain.LoadedVersionNumber=0;
                else {
                    if (float.TryParse(num, out FormMain.LoadedVersionNumber)) { } else FormMain.LoadedVersionNumber=-1;
                }
            }
    
            Cites=new List<Source>();

            // head
            for (i++; i<lines.Length; i++) {
                string line=lines[i];
                if (line=="-") break;

                string subtype = line.Substring(0, 1);
                switch (subtype) {
                    // Comment info
                    case "i":
                        Info = line.Substring(1).Replace("\\n", Environment.NewLine);
                        break;

                    case "a":
                        Author = line.Substring(1);
                        break;

                    case "d":
                        LastDate = line.Substring(1);
                        break;

                    case "f":
                        LangFrom = line.Substring(1);
                        break;

                    case "t":
                        LangLocation = line.Substring(1);
                        break;

                    case "e":
                        Select=line.Substring(1);
                        break;

                    case "c":
                        Comment=line.Substring(1).Replace("\\n", Environment.NewLine);
                        break;

                    case "b":
                        Cite=line.Substring(1).Replace("\\n", Environment.NewLine);
                      //   if (line.StartsWith("b")) {
                        string[] lines_cites=line.Substring(1).Split(new string[] { "\\n" }, StringSplitOptions.None);
                        foreach (string line_cite in lines_cites) {                             
                            Source s= new Source();
                            if (s.Load(line_cite)) { 
                                Cites.Add(s);
                            }
                        }
                        
                      //  Source.BuildSourcesIds(Cites);

                       // newLines.Add(/*line*/Source.SavePacker(Cites)); continue;
                  //  }
                        break;

                    case "z":
                        Zachytne = line.Substring(1);
                        break;

                    case "s":
                        SpadaPod = line.Substring(1);
                        break;

                    case "l":
                        LangL = line.Substring(1);
                        break;

                    case "g":
                        GPS = line.Substring(1);
                        
                        if (line.Length>3 && line.Contains(',')){
                            string[]pos=line.Substring(1).Split(',');
                            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                            ci.NumberFormat.CurrencyDecimalSeparator = ".";

                            float x=float.Parse(pos[0], NumberStyles.Any, ci);

                            float y=float.Parse(pos[1], NumberStyles.Any, ci);
                            //  if (isX && isY){
                                x=(float)Math.Round(x,5);
                                y=(float)Math.Round(y,5);
                                GPS=x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+y.ToString(System.Globalization.CultureInfo.InvariantCulture); 
                                continue;
                            //  }
                        }
                        break;

                    case "u":
                        int.TryParse(line.Substring(1), out int val);
                        Country=val;
                        break;

                    case "x":
                        {
                            //textBoxtypeLang.Text = line.Substring(1);
                            if (int.TryParse(line.Substring(1), out int ind)) {
                                TypeLang = ind;
                            } else { 
                                if (line.Substring(1)=="lokální mluva" || line.Substring(1)=="vesnická mluva") TypeLang = 3;
                                else if (line.Substring(1)=="městká mluva") TypeLang = 1;
                                else TypeLang = 0;
                            }
                        }
                        break;

                    case "q":
                        Quality = int.Parse(line.Substring(1));
                        break;

                    case "o":
                        Oblast = line.Substring(1);
                        break;

                    case "r":
                        Original = line.Substring(1);
                        break;

                    case "#":
                        break;
                }
            }

                                     
                
            // SentencePattern
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemSentencePattern s = ItemSentencePattern.Load(line); 

                if (s==null) continue;
                if (s.From=="") continue;
                itemsSentencePattern.Add(s);
            }

            // SentencePatternPart
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemSentencePatternPart s = ItemSentencePatternPart.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;

                itemsSentencePatternPart.Add(s);
            }

            // Sentences
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemSentence s = ItemSentence.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;
                
                itemsSentence.Add(s);
            }

            // SentencePart            
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemSentencePart s = ItemSentencePart.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;
                itemsSentencePart.Add(s);
            }

            // Phrase
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPhrase s = ItemPhrase.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;
                itemsPhrase.Add(s);
            }

            // SimpleWords
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemSimpleWord s = ItemSimpleWord.Load(line);                
                if (s==null) continue;

                char[] notAllowed=new char[]{'?', ' ', ';', '\t'};
                if (s.From.Contains(notAllowed)) continue;
                if (Methods.Contains(s.To, notAllowed)) continue;

                itemsSimpleWord.Add(s);
            }

            // ReplaceS
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemReplaceS s = ItemReplaceS.Load(line);
                if (s==null) continue;

                char[] notAllowed=new char[]{'?', ' ', ';', ',', '\t'};
                if (s.From.Contains(notAllowed)) continue;
                if (s.To.Contains(notAllowed)) continue;

                itemsReplaceS.Add(s);
            }

            // ReplaceG
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemReplaceG s = ItemReplaceG.Load(line);
                if (s==null) continue;

                itemsReplaceG.Add(s);
            }

            // ReplaceE
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemReplaceE s = ItemReplaceE.Load(line);
                if (s==null) continue;

                itemsReplaceE.Add(s);
            }

            //List<ItemPatternNoun> listPatternFromNoun=new List<ItemPatternNoun>(), listPatternToNoun=new List<ItemPatternNoun>();

            // PatternNounFrom
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternNoun s = ItemPatternNoun.Load(line);
                if (s==null) continue;
                if (!s.IsEmpty()) itemsPatternNounFrom.Add(s);
            }
          
            // PatternNounTo
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternNoun s = ItemPatternNoun.Load(line);
                if (s.IsEmpty()) continue;
                if (s==null) continue;
                    
                if (!s.IsEmpty()) itemsPatternNounTo.Add(s);//newLines.Add(s.SavePacker());
            }
            //List<PairTranslating> pairsNounTo = Methods.OptimizeNamesToPacker(listPatternToNoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
          //  RelocateSame(pairsNounTo);
          //  foreach (PairTranslating patternNounTo in pairsNounTo) { 
           //     if (!patternNounTo.IsDuplicate)  newLines.Add(((ItemPatternNoun)patternNounTo.Pattern).SavePacker());
           // }
          //  newLines.Add("-");

            // Noun
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemNoun s = ItemNoun.Load(line);
                if (s==null) continue;
                if (s.From.Contains("?")) continue;
                if (s.From.Contains(" ")) continue;
                if (s.From.Contains(",")) continue;
                //if (s.From=="") continue;
                // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

                /* backup
                bool pf=false;
                foreach (ItemPatternNoun f in listPatternFromNoun) { 
                    if (f.Name == s.PatternFrom) { 
                        pf=true;
                        break;
                    }
                }
                if (!pf) continue;

                bool pt = false;
                foreach (ItemPatternNoun t in listPatternToNoun) {
                    foreach (var to in s.To) {
                        if (t.Name == to.Pattern) {
                            pt = true;
                            break;
                        }
                    }
                    if (pt) break;
                }*/

                // convert names simplify (string name to "id")
                // From
               /* bool pf=false;
                ItemPatternNoun f = (ItemPatternNoun)Methods.GetOptimizedNameForPacker(pairsNounFrom, s.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    s.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<s.To.Count; to_i++) {
                    var to = s.To[to_i];
                    bool existsInside=false;
                    foreach (PairTranslating patternPair in pairsNounTo) { 
                        if (to.Pattern==patternPair.NonSimplifiedName) { 
                            to.Pattern=patternPair.Pattern.Name;
                            existsInside=true;
                            pt=true;
                            break;
                        }
                    }
                    if (!existsInside) { 
                        s.To.Remove(to);
                        to_i--;
                    }
                }                    
                if (!pt) continue;*/

                itemsNoun.Add(s);
            }
           // newLines.Add("-");

            //List<ItemPatternAdjective> listPatternFromAdjective=new List<ItemPatternAdjective>(), listPatternToAdjective=new List<ItemPatternAdjective>();

            // PatternAdjectives
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternAdjective s = ItemPatternAdjective.Load(line);
                if (s==null) continue;
                   
                if (!s.IsEmpty())  itemsPatternAdjectiveFrom.Add(s);//newLines.Add(s.SavePacker());
            }
           // List<PairTranslating> pairsAdjectiveFrom = Methods.OptimizeNamesToPacker(listPatternFromAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsAdjectiveFrom);
            //foreach (PairTranslating patternAdjectiveFrom in pairsAdjectiveFrom) { 
            //    if (!patternAdjectiveFrom.IsDuplicate) newLines.Add(((ItemPatternAdjective)patternAdjectiveFrom.Pattern).SavePacker());
            //}
           // newLines.Add("-");

            // PatternAdjectivesTo
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternAdjective s = ItemPatternAdjective.Load(line);
                if (s==null) continue;                    

                if (!s.IsEmpty()) itemsPatternAdjectiveTo.Add(s);//newLines.Add(s.SavePacker());
            }
            //List<PairTranslating> pairsAdjectiveTo = Methods.OptimizeNamesToPacker(listPatternToAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsAdjectiveTo);
            //foreach (PairTranslating patternAdjectiveTo in pairsAdjectiveTo) { 
            //    if (!patternAdjectiveTo.IsDuplicate) newLines.Add(((ItemPatternAdjective)patternAdjectiveTo.Pattern).SavePacker());
            //}

            // Adjectives
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "")continue;
                ItemAdjective s = ItemAdjective.Load(line);
                if (s==null) continue;
                //  if (s.From=="") continue;
                //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

                /*bool pf=false;
                foreach (ItemPatternAdjective f in listPatternFromAdjective) { 
                    if (f.Name == s.PatternFrom) { 
                        pf=true;
                        break;
                    }
                }
                if (!pf) continue;

                bool pt = false;
                foreach (ItemPatternAdjective t in listPatternToAdjective) {
                    foreach (var to in s.To){
                        if (t.Name == to.Pattern) {
                            pt = true;
                            break;
                        }
                    }
                    if (pt) break;
                }
                if (!pt) continue;*/

                // convert names simplify (string name to "id")
                // From
            /*    bool pf=false;
                ItemPatternAdjective f = (ItemPatternAdjective)Methods.GetOptimizedNameForPacker(pairsAdjectiveFrom, s.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    s.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<s.To.Count; to_i++) {
                    var to = s.To[to_i];
                    bool existsInside=false;
                    foreach (PairTranslating patternPair in pairsAdjectiveTo) { 
                        if (to.Pattern==patternPair.NonSimplifiedName) { 
                            to.Pattern=patternPair.Pattern.Name;
                            existsInside=true;
                            pt=true;
                            break;
                        }
                    }
                    if (!existsInside) { 
                        s.To.Remove(to);
                        to_i--;
                    }
                }                    
                if (!pt) continue;*/

                itemsAdjective.Add(s);
            }
            //newLines.Add("-");

            //List<ItemPatternPronoun> listPatternFromPronoun=new List<ItemPatternPronoun>(), listPatternToPronoun=new List<ItemPatternPronoun>();

            // PatternPronounsFrom
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternPronoun s = ItemPatternPronoun.Load(line);
                if (s==null) continue;
                
                if (!s.IsEmpty()) itemsPatternPronounFrom.Add(s);
            }
            //List<PairTranslating> pairsPronounFrom = Methods.OptimizeNamesToPacker(listPatternFromPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsPronounFrom);
            //foreach (PairTranslating patternPronounFrom in pairsPronounFrom) { 
            //    if (!patternPronounFrom.IsDuplicate) newLines.Add(((ItemPatternPronoun)patternPronounFrom.Pattern).SavePacker());
            //}

            // PatternPronounsTo
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternPronoun s = ItemPatternPronoun.Load(line);
                if (s==null) continue;
                    
                if (!s.IsEmpty()) itemsPatternPronounTo.Add(s);
            }
            //List<PairTranslating> pairsPronounTo = Methods.OptimizeNamesToPacker(listPatternToPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsPronounTo);
            //foreach (PairTranslating patternPronounTo in pairsPronounTo) { 
            //    if (!patternPronounTo.IsDuplicate) newLines.Add(((ItemPatternPronoun)patternPronounTo.Pattern).SavePacker());
            //}

            // Pronouns
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "")  continue;
                ItemPronoun s = ItemPronoun.Load(line);
                if (s==null) continue;
                //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

                /*bool pf=false;
                foreach (ItemPatternPronoun f in listPatternFromPronoun) { 
                    if (f.Name == s.PatternFrom) { 
                        pf=true;
                        break;
                    }
                }
                if (!pf) continue;

                bool pt = false;
                foreach (ItemPatternPronoun t in listPatternToPronoun) {
                    foreach (var to in s.To){
                        if (t.Name == to.Pattern) {
                            pt = true;
                            break;
                        }
                    }
                    if (pt) break;
                }
                if (!pt) continue;*/
                // convert names simplify (string name to "id")
                // From
               /* bool pf=false;
                ItemPatternPronoun f = (ItemPatternPronoun)Methods.GetOptimizedNameForPacker(pairsPronounFrom, s.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    s.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<s.To.Count; to_i++) {
                    var to = s.To[to_i];
                    bool existsInside=false;
                    foreach (PairTranslating patternPair in pairsPronounTo) { 
                        if (to.Pattern==patternPair.NonSimplifiedName) { 
                            to.Pattern=patternPair.Pattern.Name;
                            existsInside=true;
                            pt=true;
                            break;
                        }
                    }
                    if (!existsInside) { 
                        s.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;*/

                itemsPronoun.Add(s);
            }

            //List<ItemPatternNumber> listPatternFromNumber=new List<ItemPatternNumber>(), listPatternToNumber=new List<ItemPatternNumber>();

            // PatternNumbersFrom
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "")  continue;
                ItemPatternNumber s = ItemPatternNumber.Load(line);
                if (s==null) continue;
                    
                if (!s.IsEmpty()) itemsPatternNumberFrom.Add(s);
            }
            //List<PairTranslating> pairsNumberFrom = Methods.OptimizeNamesToPacker(listPatternFromNumber.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsNumberFrom);
            //foreach (PairTranslating patternNumberFrom in pairsNumberFrom) { 
            //    if (!patternNumberFrom.IsDuplicate) newLines.Add(((ItemPatternNumber)patternNumberFrom.Pattern).SavePacker());
            //}

            // PatternNumbersTo
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternNumber s = ItemPatternNumber.Load(line);
                if (s==null) continue;
                    
                if (!s.IsEmpty()) itemsPatternNumberTo.Add(s);//newLines.Add(s.SavePacker());
            }
            //List<PairTranslating> pairsNumberTo = Methods.OptimizeNamesToPacker(listPatternToNumber.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsNounTo);
            //foreach (PairTranslating patternNumberTo in pairsNumberTo) { 
             //   if (!patternNumberTo.IsDuplicate) newLines.Add(((ItemPatternNumber)patternNumberTo.Pattern).SavePacker());
            //}

            // Numbers
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemNumber s = ItemNumber.Load(line);
                if (s==null) continue;
                //  if (s.From=="") continue;
                //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

                /*bool pf=false;
                foreach (ItemPatternNumber f in listPatternFromNumber) { 
                    if (f.Name == s.PatternFrom) { 
                        pf=true;
                        break;
                    }
                }
                if (!pf) continue;
                bool pt = false;
                foreach (ItemPatternNumber t in listPatternToNumber) {
                    foreach (var to in s.To) {
                        if (t.Name == to.Pattern) {
                            pt = true;
                            break;
                        }
                    }
                    if (pt) break;
                }
                if (!pt) continue;*/
                // convert names simplify (string name to "id")
                // From
              /*  bool pf=false;
                ItemPatternNumber f = (ItemPatternNumber)Methods.GetOptimizedNameForPacker(pairsNumberFrom, s.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    s.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<s.To.Count; to_i++) {
                    var to = s.To[to_i];
                    bool existsInside=false;
                    foreach (PairTranslating patternPair in pairsNumberTo) { 
                        if (to.Pattern==patternPair.NonSimplifiedName) { 
                            to.Pattern=patternPair.Pattern.Name;
                            existsInside=true;
                            pt=true;
                            break;
                        }
                    }
                    if (!existsInside) { 
                        s.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;*/

                itemsNumber.Add(s);
            }

            List<ItemPatternVerb> listPatternFromVerb=new List<ItemPatternVerb>(), listPatternToVerb=new List<ItemPatternVerb>();

            // PatternVerbsFrom
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternVerb s = ItemPatternVerb.Load(line);
                if (s==null) continue;
                if (s.Name=="") continue;
                  
                if (!s.IsEmpty()) itemsPatternVerbFrom.Add(s);
            }
            //List<PairTranslating> pairsVerbFrom = Methods.OptimizeNamesToPacker(listPatternFromVerb.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsVerbFrom);
            //foreach (PairTranslating patternVerbFrom in pairsVerbFrom) { 
            //    if (!patternVerbFrom.IsDuplicate) newLines.Add(((ItemPatternVerb)patternVerbFrom.Pattern).SavePacker());
            //}

            // PatternVerbsTo
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPatternVerb s = ItemPatternVerb.Load(line);
                if (s==null) continue;
                if (s.Name=="") continue;
                    
                if (!s.IsEmpty()) itemsPatternVerbTo.Add(s);
            }
            //List<PairTranslating> pairsVerbTo = Methods.OptimizeNamesToPacker(listPatternToVerb.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            //RelocateSame(pairsVerbTo);
            //foreach (PairTranslating patternVerbTo in pairsVerbTo) { 
            //    if (!patternVerbTo.IsDuplicate) newLines.Add(((ItemPatternVerb)patternVerbTo.Pattern).SavePacker());
            //}

            // Verb
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemVerb s = ItemVerb.Load(line);
                if (s==null) continue;
                char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                //  if (s.From=="") continue;
                if (s.From.Contains(notAllowed)) continue;
                // if (Methods.ContainsBody(s.To, notAllowed)) continue;

                /*bool pf = false;
                foreach (ItemPatternVerb f in listPatternFromVerb) {
                    if (f.Name == s.PatternFrom) {
                        pf = true;
                        break;
                    }
                }
                if (!pf) continue;

                bool pt = false;
                foreach (ItemPatternVerb t in listPatternToVerb) {
                    foreach (var to in s.To){
                        if (t.Name == to.Pattern) {
                            pt = true;
                            break;
                        }
                    }
                    if (pt) break;
                }
                if (!pt) continue;*/
                // convert names simplify (string name to "id")
                // From
               /* bool pf=false;
                ItemPatternVerb f = (ItemPatternVerb)Methods.GetOptimizedNameForPacker(pairsVerbFrom, s.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    s.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<s.To.Count; to_i++) {
                    var to = s.To[to_i];
                    bool existsInside=false;
                    foreach (PairTranslating patternPair in pairsVerbTo) {
                        if (to.Pattern==patternPair.NonSimplifiedName) { 
                            to.Pattern=patternPair.Pattern.Name;
                            existsInside=true;
                            pt=true;
                            break;
                        }
                    }
                    if (!existsInside) { 
                        s.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;*/

                itemsVerb.Add(s);
            }

            // Adverb
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemAdverb s = ItemAdverb.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;
                if (LangLocation=="Lichnov") 
                    Debug.WriteLine(lines.Length);
                itemsAdverb.Add(s);
            }

            // Preposition
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "")  continue;
                ItemPreposition s = ItemPreposition.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;
                itemsPreposition.Add(s);
            }

            // ConjunctionConjunction
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-")  break;
                if (line == "")  continue;
                ItemConjunction s = ItemConjunction.Load(line);
                if (s==null) continue;
                if (s.From=="") continue;

                itemsConjunction.Add(s);
            }

            // Particle
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-")  break;
                if (line == "")  continue;
                ItemParticle s = ItemParticle.Load(line);
                if (s.From=="") continue;

                itemsParticle.Add(s);
            }

            // Interjection
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "")  continue;
                ItemInterjection s = ItemInterjection.Load(line);
                if (s.From=="") continue;

                itemsInterjection.Add(s);
            }

            // PhrasePattern
            for (i++; i < lines.Length; i++) {
                string line = lines[i];
                if (line == "-") break;
                if (line == "") continue;
                ItemPhrasePattern s = ItemPhrasePattern.Load(line);
                itemsPhrasePattern.Add(s);
            }

            FormMain.LoadedVersionNumber=_LoadedVersionNumber;
            FormMain.LoadedSaveVersion=_LoadedSaveVersion;
        }     
       
        //List<string> GetOptimizedForPacker() { 
        //    List<string> newLines=new List<string>();            
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
        //        //  if (line.StartsWith("t")) continue;
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
        //                //  if (isX && isY){
        //                    x=(float)Math.Round(x,5);
        //                    y=(float)Math.Round(y,5);
        //                    newLines.Add("g"+x.ToString(System.Globalization.CultureInfo.InvariantCulture)+","+y.ToString(System.Globalization.CultureInfo.InvariantCulture)); 
        //                    continue;
        //                //  }
        //            }
        //        }
        //    }
        //    newLines.Add("-");                              
                
        //    // SentencePattern
        //    foreach (ItemSentencePattern item in itemsSentencePattern) {                   
        //        if (item.From=="") continue;
        //        newLines.Add(item.Save());
        //    }
        //    newLines.Add("-");

        //    // SentencePatternPart
        //    foreach (ItemSentencePatternPart item in itemsSentencePatternPart) {              
        //        if (item.From=="") continue;
        //        newLines.Add(item.Save());
        //    }
        //    newLines.Add("-");

        //    // Sentences
        //    foreach (ItemSentence item in itemsSentence) {
        //        if (item.From=="") continue;

        //        string saveP=item.SavePacker(Cites);
        //        if (saveP!=null)newLines.Add(saveP);
        //    }
        //    newLines.Add("-");

        //    // SentencePart
        //    foreach (ItemSentencePart item in itemsSentencePart) {    
        //        if (item.From=="") continue;
        //        string saveP=item.SavePacker(Cites);
        //        if (saveP!=null)newLines.Add(saveP);
        //    }
        //    newLines.Add("-");

        //    // Phrase
        //    foreach (ItemPhrase item in itemsPhrase) {  
        //        char[] notAllowed=new char[]{'?', ';', '\t'};
        //        if (Methods.Contains(item.From, notAllowed)) continue;
        //        if (Methods.Contains(item.To, notAllowed)) continue;
        //        string saveP=item.SavePacker(Cites);
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
        //        // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

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
        //        //  if (s.From=="") continue;
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

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
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

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
        //            // newLines.Add(s.SavePacker());
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
        //        //  if (s.From=="") continue;
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        //  if (Methods.ContainsBody(s.To, notAllowed)) continue;

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
        //        //  s.Infinitive=Methods.SavePackerStringMultiple()
                  
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
        //        //  if (s.From=="") continue;
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
        //        // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        if (s.From=="") continue;
        //        // if (s.From.Contains(notAllowed)) continue;
        //        // if (Methods.Contains(s.To, notAllowed)) continue;
                    
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
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        //  if (s.From.Contains(notAllowed)) continue;
        //        // if (Methods.Contains(s.To, notAllowed)) continue;
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
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        // if (s.From.Contains(notAllowed)) continue;
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
        //        // char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        // if (s.From.Contains(notAllowed)) continue;
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
        //        //  char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
        //        if (s.From=="") continue;
        //        // if (s.From.Contains(notAllowed)) continue;
        //        //if (Methods.Contains(s.To, notAllowed)) continue;

        //        newLines.Add(s.SavePacker(Cites));
        //    }
        //    newLines.Add("-");

        //    // PhrasePattern
        //    foreach (ItemPhrasePattern item in itemsPhrasePattern) { 
        //        if (item.From=="") continue;
        //        newLines.Add(item.Save());
        //    }

        //    return newLines;
        //}



    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace TranslatorWritter {
    // FormMain (click save package) -> Lang class (load all files as) -> ComprimeLang class (parts of strings for optimize between files) -> Packer class (save)
    public class SavedPatternData{ 
        public string PatternName, PatternData;
        public SameLine Shortcut;
    }
    
    public class SavedStringData{ 
        public string Data;
        public SameLine Shortcut;
    }

    internal class ComprimeLang{
        public List<SavedStringData> itemsSimpleWord=new List<SavedStringData>(), itemsPhrase=new List<SavedStringData>(), itemsSentencePart=new List<SavedStringData>(), itemsSentence=new List<SavedStringData>();
        public List<SavedStringData> itemsReplaceS=new List<SavedStringData>(), itemsReplaceG=new List<SavedStringData>(), itemsReplaceE=new List<SavedStringData>();
        public List<SavedStringData> itemsPhrasePattern=new List<SavedStringData>();
        public List<SavedStringData> itemsSentencePatternPart=new List<SavedStringData>();
        public List<SavedStringData> itemsSentencePattern=new List<SavedStringData>();
        public List<SavedStringData> itemsConjunction=new List<SavedStringData>(), itemsParticle=new List<SavedStringData>(), itemsInterjection=new List<SavedStringData>(), itemsAdverb=new List<SavedStringData>(), itemsPreposition=new List<SavedStringData>();
        public List<SavedStringData> itemsNoun=new List<SavedStringData>(), itemsAdjective=new List<SavedStringData>(), itemsPronoun=new List<SavedStringData>(), itemsNumber=new List<SavedStringData>(), itemsVerb=new List<SavedStringData>();
        public List<SavedPatternData> itemsPatternNounFrom=new List<SavedPatternData>(), itemsPatternNounTo=new List<SavedPatternData>();
        public List<SavedPatternData> itemsPatternPronounFrom=new List<SavedPatternData>(), itemsPatternPronounTo=new List<SavedPatternData>();
        public List<SavedPatternData> itemsPatternAdjectiveFrom=new List<SavedPatternData>(), itemsPatternAdjectiveTo=new List<SavedPatternData>();
        public List<SavedPatternData> itemsPatternVerbFrom=new List<SavedPatternData>(), itemsPatternVerbTo=new List<SavedPatternData>();
        public List<SavedPatternData> itemsPatternNumberFrom=new List<SavedPatternData>(), itemsPatternNumberTo=new List<SavedPatternData>();

        string Info;        
        private int Country;
        private string GPS;
        private string SpadaPod;
        private string Oblast;
        private string Comment;
        private int Quality;
        private string Original;
        private string Zachytne;
        private string Author;
        private string LastDate;
        private string Cite;
        private string Lang;
        private string LangLocation;
        private string LangFrom;
        private string Select;
        public object TypeLang;
        public string Cites;

        public object this[string name] {
            get {
                var properties = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (var property in properties) {
                    if (property.Name == "items"+name) return property.GetValue(this);
                }

                throw new ArgumentException("Can't find property");
            }
            set {
                return;
            }
        }

        public ComprimeLang(Lang lang) {          
            

            Info=lang.Info;       
            Country=lang.Country;
            GPS=lang.GPS;
           // SpadaPod=lang.SpadaPod;
            Oblast=lang.Oblast;
            Comment=lang.Comment;
            Quality=lang.Quality;
            Original=lang.Original;
           // Zachytne=lang.Zachytne
           // Author=lang.
          //  LastDate=lang.;
            Cite=lang.Cite;
        //    Lang=lang.
            LangLocation=lang.LangLocation;
            LangFrom=lang.LangFrom;
            Select=lang.Select;
            TypeLang=lang.TypeLang;

               
            //Cites=new List<string>();            
            Source.BuildSourcesIds(lang.Cites);
            Cites=Source.SavePacker(lang.Cites);
            

            // SentencePattern
            foreach (ItemSentencePattern item in lang.itemsSentencePattern) {                   
                if (item.From=="") continue;
                itemsSentencePattern.Add(new SavedStringData{Data=item.Save()});
            }

            // SentencePatternPart
            foreach (ItemSentencePatternPart item in lang.itemsSentencePatternPart) {              
                if (item.From=="") continue;
                itemsSentencePatternPart.Add(new SavedStringData{Data=item.Save() });
            }

            // Sentences
            foreach (ItemSentence item in lang.itemsSentence) {
                if (item.From=="") continue;

                string saveP=item.SavePacker(lang.Cites);
                if (saveP!=null) itemsSentence.Add(new SavedStringData{Data=saveP });
            }

            // SentencePart
            foreach (ItemSentencePart item in lang.itemsSentencePart) {    
                if (item.From=="") continue;
                string saveP=item.SavePacker(lang.Cites);
                if (saveP!=null) itemsSentencePart.Add(new SavedStringData{Data=saveP });
            }

            // Phrase
            foreach (ItemPhrase item in lang.itemsPhrase) {  
                char[] notAllowed=new char[]{'?', ';', '\t'};
                if (Methods.Contains(item.From, notAllowed)) continue;
                if (Methods.Contains(item.To, notAllowed)) continue;
                string saveP=item.SavePacker(lang.Cites);
                if (saveP!=null) itemsPhrase.Add(new SavedStringData{Data=saveP });
            }

            // SimpleWords
            foreach (ItemSimpleWord item in lang.itemsSimpleWord) {
                char[] notAllowed=new char[]{'?', ' ', ';', '\t'};
                if (item.From.Contains(notAllowed)) continue;
                if (Methods.Contains(item.To, notAllowed)) continue;

                string saveP=item.SavePacker(lang.Cites);
                if (saveP!=null) itemsSimpleWord.Add(new SavedStringData{Data=saveP });
            }

            // ReplaceS
            foreach (ItemReplaceS item in lang.itemsReplaceS) {               
                char[] notAllowed=new char[]{'?', ' ', ';', ',', '\t'};
                if (item.From.Contains(notAllowed)) continue;
                if (item.To.Contains(notAllowed)) continue;

                itemsReplaceS.Add(new SavedStringData{Data=item.Save() });
            }

            // ReplaceG
            foreach (ItemReplaceG item in lang.itemsReplaceG) {
                itemsReplaceG.Add(new SavedStringData{Data=item.Save() });
            }

            // ReplaceE
            foreach (ItemReplaceE item in lang.itemsReplaceE) {
                itemsReplaceE.Add(new SavedStringData{Data=item.Save() });
            }


            List<ItemPatternNoun> listPatternFromNoun=new List<ItemPatternNoun>(), listPatternToNoun=new List<ItemPatternNoun>();
            // PatternNounFrom
            foreach (ItemPatternNoun item in lang.itemsPatternNounFrom) {
                if (!item.IsEmpty()) listPatternFromNoun.Add(item);
            }
            // optimize names
            List<PairTranslating> pairsNounFrom = Methods.OptimizeNamesToPacker(listPatternFromNoun.ConvertAll(p => p as ItemTranslatingPattern));                
            // remove same ones
            PairTranslating.RelocateSame(pairsNounFrom);
            // save
            foreach (PairTranslating patternNounFrom in pairsNounFrom) { 
                if (!patternNounFrom.IsDuplicate) itemsPatternNounFrom.Add(((ItemPatternNoun)patternNounFrom.Pattern).SaveComprimeLang());
            }

            // PatternNounTo
            foreach (ItemPatternNoun item in lang.itemsPatternNounTo) {
                if (!item.IsEmpty()) listPatternToNoun.Add(item);
            }
            List<PairTranslating> pairsNounTo = Methods.OptimizeNamesToPacker(listPatternToNoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsNounTo);
            foreach (PairTranslating patternNounTo in pairsNounTo) { 
                if (!patternNounTo.IsDuplicate)  itemsPatternNounTo.Add(((ItemPatternNoun)patternNounTo.Pattern).SaveComprimeLang());
            }

            // Noun
            foreach (ItemNoun item in lang.itemsNoun) {               
                if (item.From.Contains("?")) continue;
                if (item.From.Contains(" ")) continue;
                if (item.From.Contains(",")) continue;             

                // convert names simplify (string name to "id")
                // From
                bool pf=false;
                ItemPatternNoun f = (ItemPatternNoun)Methods.GetOptimizedNameForPacker(pairsNounFrom, item.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    item.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<item.To.Count; to_i++) {
                    var to = item.To[to_i];
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
                        item.To.Remove(to);
                        to_i--;
                    }
                }                    
                if (!pt) continue;

                itemsNoun.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            List<ItemPatternAdjective> listPatternFromAdjective=new List<ItemPatternAdjective>(), listPatternToAdjective=new List<ItemPatternAdjective>();

            // PatternAdjectivesFrom
            foreach (ItemPatternAdjective item in lang.itemsPatternAdjectiveFrom) {              
                if (!item.IsEmpty())  listPatternFromAdjective.Add(item);
            }
            List<PairTranslating> pairsAdjectiveFrom = Methods.OptimizeNamesToPacker(listPatternFromAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsAdjectiveFrom);
            foreach (PairTranslating patternAdjectiveFrom in pairsAdjectiveFrom) { 
                if (!patternAdjectiveFrom.IsDuplicate) itemsPatternAdjectiveFrom.Add(((ItemPatternAdjective)patternAdjectiveFrom.Pattern).SaveComprimeLang());
            }

            // PatternAdjectivesTo
            foreach (ItemPatternAdjective item in lang.itemsPatternAdjectiveTo) {
                if (!item.IsEmpty()) listPatternToAdjective.Add(item);
            }
            List<PairTranslating> pairsAdjectiveTo = Methods.OptimizeNamesToPacker(listPatternToAdjective.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsAdjectiveTo);
            foreach (PairTranslating patternAdjectiveTo in pairsAdjectiveTo) { 
                if (!patternAdjectiveTo.IsDuplicate) itemsPatternAdjectiveTo.Add(((ItemPatternAdjective)patternAdjectiveTo.Pattern).SaveComprimeLang());
            }

            // Adjectives
            foreach (ItemAdjective item in lang.itemsAdjective) {
                // convert names simplify (string name to "id")
                // From
                bool pf=false;
                ItemPatternAdjective f = (ItemPatternAdjective)Methods.GetOptimizedNameForPacker(pairsAdjectiveFrom, item.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    item.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<item.To.Count; to_i++) {
                    var to = item.To[to_i];
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
                        item.To.Remove(to);
                        to_i--;
                    }
                }                    
                if (!pt) continue;

                itemsAdjective.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            List<ItemPatternPronoun> listPatternFromPronoun=new List<ItemPatternPronoun>(), listPatternToPronoun=new List<ItemPatternPronoun>();

            // PatternPronounsFrom
            foreach (ItemPatternPronoun item in lang.itemsPatternPronounFrom) {             
                if (!item.IsEmpty()) listPatternFromPronoun.Add(item);
            }
            List<PairTranslating> pairsPronounFrom = Methods.OptimizeNamesToPacker(listPatternFromPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsPronounFrom);
            foreach (PairTranslating patternPronounFrom in pairsPronounFrom) { 
                if (!patternPronounFrom.IsDuplicate) itemsPatternPronounFrom.Add(((ItemPatternPronoun)patternPronounFrom.Pattern).SaveComprimeLang());
            }

            // PatternPronounsTo
            foreach (ItemPatternPronoun item in lang.itemsPatternPronounTo) {
                if (!item.IsEmpty()) listPatternToPronoun.Add(item);
            }
            List<PairTranslating> pairsPronounTo = Methods.OptimizeNamesToPacker(listPatternToPronoun.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsPronounTo);
            foreach (PairTranslating patternPronounTo in pairsPronounTo) { 
                if (!patternPronounTo.IsDuplicate) itemsPatternPronounTo.Add(((ItemPatternPronoun)patternPronounTo.Pattern).SaveComprimeLang());
            }
           

            // Pronoun
            foreach (ItemPronoun item in lang.itemsPronoun) {             
                bool pf=false;
                ItemPatternPronoun f = (ItemPatternPronoun)Methods.GetOptimizedNameForPacker(pairsPronounFrom, item.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    item.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<item.To.Count; to_i++) {
                    var to = item.To[to_i];
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
                        item.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;

                itemsPronoun.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }


            List<ItemPatternNumber> listPatternFromNumber=new List<ItemPatternNumber>(), listPatternToNumber=new List<ItemPatternNumber>();
            // PatternNumbersFrom
            foreach (ItemPatternNumber item in lang.itemsPatternNumberFrom) {
                if (!item.IsEmpty()) listPatternFromNumber.Add(item);
            }
            List<PairTranslating> pairsNumberFrom = Methods.OptimizeNamesToPacker(listPatternFromNumber.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsNumberFrom);
            foreach (PairTranslating patternNumberFrom in pairsNumberFrom) { 
                if (!patternNumberFrom.IsDuplicate) itemsPatternNumberFrom.Add(((ItemPatternNumber)patternNumberFrom.Pattern).SaveComprimeLang());
            }

            // PatternNumbersTo
            foreach (ItemPatternNumber item in lang.itemsPatternNumberTo) {
                if (item==null) continue;                    
                if (!item.IsEmpty()) listPatternToNumber.Add(item);
            }
            List<PairTranslating> pairsNumberTo = Methods.OptimizeNamesToPacker(listPatternToNumber.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsNounTo);
            foreach (PairTranslating patternNumberTo in pairsNumberTo) { 
                if (!patternNumberTo.IsDuplicate) itemsPatternNumberTo.Add(((ItemPatternNumber)patternNumberTo.Pattern).SaveComprimeLang());
            }

            // Numbers
            foreach (ItemNumber item in lang.itemsNumber) {               
               
                // From
                bool pf=false;
                ItemPatternNumber f = (ItemPatternNumber)Methods.GetOptimizedNameForPacker(pairsNumberFrom, item.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    item.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<item.To.Count; to_i++) {
                    var to = item.To[to_i];
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
                        item.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;

                itemsNumber.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }


            List<ItemPatternVerb> listPatternFromVerb=new List<ItemPatternVerb>(), listPatternToVerb=new List<ItemPatternVerb>();

            // PatternVerbsFrom
            foreach (ItemPatternVerb item in lang.itemsPatternVerbFrom) { 
                if (item.Name=="") continue;                   
                if (!item.IsEmpty()) listPatternFromVerb.Add(item);
            }
            List<PairTranslating> pairsVerbFrom = Methods.OptimizeNamesToPacker(listPatternFromVerb.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsVerbFrom);
            foreach (PairTranslating patternVerbFrom in pairsVerbFrom) { 
                if (!patternVerbFrom.IsDuplicate) itemsPatternVerbFrom.Add(((ItemPatternVerb)patternVerbFrom.Pattern).SaveComprimeLang());
            }

            // PatternVerbsTo
            foreach (ItemPatternVerb item in lang.itemsPatternVerbTo) {
                if (item.Name=="") continue;
                if (!item.IsEmpty()) listPatternToVerb.Add(item);
            }
            List<PairTranslating> pairsVerbTo = Methods.OptimizeNamesToPacker(listPatternToVerb.ConvertAll(p => p as ItemTranslatingPattern));
            // remove same ones
            PairTranslating.RelocateSame(pairsVerbTo);
            foreach (PairTranslating patternVerbTo in pairsVerbTo) { 
                if (!patternVerbTo.IsDuplicate) itemsPatternVerbTo.Add(((ItemPatternVerb)patternVerbTo.Pattern).SaveComprimeLang());
            }

            // Verb
            foreach (ItemVerb item in lang.itemsVerb) {
                char[] notAllowed=new char[]{'?', ' ', ';', '_', '|', '-', '\t'};
                if (item.From.Contains(notAllowed)) continue;
              
                // From
                bool pf=false;
                ItemPatternVerb f = (ItemPatternVerb)Methods.GetOptimizedNameForPacker(pairsVerbFrom, item.PatternFrom);
                if (f!=null) { 
                    pf=true;
                    item.PatternFrom=f.Name;
                }                    
                if (!pf) continue;

                // To
                bool pt = false;
                for (int to_i=0; to_i<item.To.Count; to_i++) {
                    var to = item.To[to_i];
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
                        item.To.Remove(to);
                        to_i--;
                    }
                }
                if (!pt) continue;

                itemsVerb.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            // Adverb
            foreach (ItemAdverb item in lang.itemsAdverb) {
                if (item==null) continue;
                if (item.From=="") continue;               
                itemsAdverb.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            // Preposition
            foreach (ItemPreposition item in lang.itemsPreposition) {
                if (item.From=="") continue;
                string data=item.SavePacker(lang.Cites);
                if (data!=null) itemsPreposition.Add(new SavedStringData{Data=data });
            }

            // Conjunction
            foreach (ItemConjunction item in lang.itemsConjunction) {
                if (item.From=="") continue;
                itemsConjunction.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            // Particle
            foreach (ItemParticle item in lang.itemsParticle) {
                if (item.From=="") continue;              
                itemsParticle.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            // Interjection
            foreach (ItemInterjection item in lang.itemsInterjection) {
                if (item.From=="") continue;
                itemsInterjection.Add(new SavedStringData{Data=item.SavePacker(lang.Cites) });
            }

            // PhrasePattern
            foreach (ItemPhrasePattern item in lang.itemsPhrasePattern) { 
                if (item.From=="") continue;
                itemsPhrasePattern.Add(new SavedStringData{Data=item.Save() });
            }
        }


        public List<string> GetSavedLinesForPacker() {
            List<string> newLines=new List<string>();            
            //List<Source> Cites=new List<Source>();

            // head
            if (LangLocation!=null) newLines.Add("t"+LangLocation);            
            if (Comment!=null) newLines.Add("c"+Comment.Replace(Environment.NewLine, "\\n"));
            if (Cites!=null) newLines.Add("b" + Cites);           
            if (Oblast!=null) newLines.Add("o"+Oblast);
            if (Quality!=-1) newLines.Add("q"+Quality);
            if (Country!=-1) newLines.Add("u"+Country);
            if (Original!=null) newLines.Add("r"+Original);
            if (GPS!=null) newLines.Add("g"+GPS);              
        
            newLines.Add("-");                              
                
            // SentencePattern
            WriteSavedStringData(itemsSentencePattern);
            newLines.Add("-");

            // SentencePatternPart
            WriteSavedStringData(itemsSentencePatternPart);
            newLines.Add("-");

            // Sentences
            WriteSavedStringData(itemsSentence);
            newLines.Add("-");

            // SentencePart
            WriteSavedStringData(itemsSentencePart);
            newLines.Add("-");
           /* foreach (var sl in itemsSentencePart) { 
                if (sl.Shortcut.Id>=9) throw new Exception();
            }*/

            // Phrase
            WriteSavedStringData(itemsPhrase);
            newLines.Add("-");

            // SimpleWords
            WriteSavedStringData(itemsSimpleWord);
            newLines.Add("-");

            // ReplaceS
            WriteSavedStringData(itemsReplaceS);
            newLines.Add("-");

            // ReplaceG
            WriteSavedStringData(itemsReplaceG);
            newLines.Add("-");

            // ReplaceE
            WriteSavedStringData(itemsReplaceE);
            newLines.Add("-");

            // PatternNounFrom
            WriteSavedPatternData(itemsPatternNounFrom);
            newLines.Add("-");

            // PatternNounTo
            WriteSavedPatternData(itemsPatternNounTo);
            newLines.Add("-");

            // Noun
            WriteSavedStringDataNoOptimize(itemsNoun);
            newLines.Add("-");

            // PatternAdjectives
            WriteSavedPatternData(itemsPatternAdjectiveFrom);           
            newLines.Add("-");

            // PatternAdjectivesTo
            WriteSavedPatternData(itemsPatternAdjectiveTo);
            newLines.Add("-");

            // Adjectives
            WriteSavedStringDataNoOptimize(itemsAdjective);
            newLines.Add("-");

            // PatternPronounsFrom
            WriteSavedPatternData(itemsPatternPronounFrom);
            newLines.Add("-");

            // PatternPronounsTo
            WriteSavedPatternData(itemsPatternPronounTo);
            newLines.Add("-");

            // Pronouns
            WriteSavedStringDataNoOptimize(itemsPronoun);
            newLines.Add("-");

            // PatternNumbersFrom
            WriteSavedPatternData(itemsPatternNumberFrom);
            newLines.Add("-");

            // PatternNumbersTo
            WriteSavedPatternData(itemsPatternNumberTo);
            newLines.Add("-");

            // Numbers
            WriteSavedStringDataNoOptimize(itemsNumber);
            newLines.Add("-");

            // PatternVerbsFrom
            WriteSavedPatternData(itemsPatternVerbFrom);
            newLines.Add("-");

            // PatternVerbsTo
            WriteSavedPatternData(itemsPatternVerbTo);            
            newLines.Add("-");

            // Verb
            WriteSavedStringDataNoOptimize(itemsVerb);
            newLines.Add("-");

            // Adverb
            WriteSavedStringData(itemsAdverb);
            newLines.Add("-");

            // Preposition
            WriteSavedStringData(itemsPreposition);
            newLines.Add("-");

            // Conjunction
            WriteSavedStringData(itemsConjunction);
            newLines.Add("-");

            // Particle
            WriteSavedStringData(itemsParticle);
            newLines.Add("-");

            // Interjection
            WriteSavedStringData(itemsInterjection);
            newLines.Add("-");

            // PhrasePattern
            WriteSavedStringData(itemsPhrasePattern);

            return newLines;
            
            void WriteSavedStringDataNoOptimize(List<SavedStringData> list) {
                foreach (SavedStringData item in list) {
                    newLines.Add(item.Data);
                }
            }

            void WriteSavedStringData(List<SavedStringData> list) {
                foreach (SavedStringData item in list) {
                    if (item.Shortcut!=null) newLines.Add(item.Shortcut.getShortcut());
                    else newLines.Add(item.Data);
                }
            }

            void WriteSavedPatternData(List<SavedPatternData> list) {
                foreach (SavedPatternData item in list) {
                    if (item.Shortcut!=null) newLines.Add(item.PatternName+item.Shortcut.getShortcut());
                    else newLines.Add(item.PatternName+"|"+item.PatternData);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse;
using System.Reflection.Emit;

namespace Dolphus.RimBuzzer
{
    static class Tools
    {

        public static void CheckboxEnhanced(this Listing_Standard listing, string name, ref bool value, string tooltip = null)
        {
            var startHeight = listing.CurHeight;

            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            listing.CheckboxLabeled((name + "Title").Translate(), ref value);

            Text.Font = GameFont.Tiny;
            listing.ColumnWidth -= 34;
            GUI.color = Color.gray;
            _ = listing.Label((name + "Explained").Translate());
            listing.ColumnWidth += 34;

            var rect = listing.GetRect(0);
            rect.height = listing.CurHeight - startHeight;
            rect.y -= rect.height;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                if (!tooltip.NullOrEmpty())
                    TooltipHandler.TipRegion(rect, tooltip);
            }
        }

        public static void IntegerLabeled(this Listing_Standard listing, string name, ref int value, string tooltip = null)
        {
            var startHeight = listing.CurHeight;

            var rect = listing.GetRect("Hg".GetHeightCached() + listing.verticalSpacing);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            var savedAnchor = Text.Anchor;

            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect, (name + "Title").Translate());

            rect.xMin += rect.width * 2 / 3;
            var newValue = Widgets.TextField(rect, value.ToString());
            if (int.TryParse(newValue, out var newInteger))
                value = newInteger;

            Text.Anchor = savedAnchor;

            var key = name + "Explained";
            if (key.CanTranslate())
            {
                Text.Font = GameFont.Tiny;
                listing.ColumnWidth -= 34;
                GUI.color = Color.gray;
                _ = listing.Label(key.Translate());
                listing.ColumnWidth += 34;
            }

            rect = listing.GetRect(0);
            rect.height = listing.CurHeight - startHeight;
            rect.y -= rect.height;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                if (!tooltip.NullOrEmpty())
                    TooltipHandler.TipRegion(rect, tooltip);
            }
        }
        /// <summary>
        /// A mod settings method to add an inline checkbox and a - int + field that appears conditionally on whether or not the checkbox is ticked. A description appears below.
        /// To use just the - int + field, use the overloaded method IntegerPlusMinus.
        /// </summary>
        /// <param name="listing"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="checkOn"></param>
        /// <param name="tooltip"></param>
        /// <param name="valMin"></param>
        /// <param name="valMax"></param>
        /// <param name="checkBox"></param>
        public static void IntegerPlusMinusCheckbox(this Listing_Standard listing, string name, ref int value, ref bool checkOn, string tooltip = null, int valMin = 1, int valMax = 60, bool checkBox = true, bool fieldOnlyIfCheck = true)
        {
            var startHeight = listing.CurHeight;

            var rect = listing.GetRect("Hg".GetHeightCached() + listing.verticalSpacing);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            var savedAnchor = Text.Anchor;

            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect, (name + "Title").Translate());

            rect.xMin += rect.width * 2 / 3; // I want a minus and plus button as well. Now we have the rect extremes.

            const float buttonWidth = 30f;
            const float buttonSpace = 4f;
            const float checkBoxSpace = 22f;
            float savedxMin = rect.xMin; float savedxMax = rect.xMax;
            Text.Anchor = TextAnchor.MiddleCenter; // GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            if (checkBox)
            {
                if (!listing.BoundingRectCached.HasValue || rect.Overlaps(listing.BoundingRectCached.Value))
                {
                    rect.xMin = savedxMin - checkBoxSpace - buttonSpace;
                    rect.xMax = savedxMin - buttonSpace;
                    Widgets.CheckboxLabeled(rect, (name + "Checkbox"), ref checkOn);
                    rect.xMin = savedxMin; rect.xMax = savedxMax;
                }
            }
            // else checkOn = true; // If checkBox is false, I still want to draw the - int + field. 

            if (!fieldOnlyIfCheck || checkOn) 
            {
                rect.xMax = rect.xMin + buttonWidth;
                if (Widgets.ButtonText(rect, "-") && value > valMin)
                    value -= 1;

                rect.xMax = savedxMax;
                rect.xMin = rect.xMax - buttonWidth;
                if (Widgets.ButtonText(rect, "+") && value < valMax)
                    value += 1;

                Text.Anchor = TextAnchor.MiddleLeft;
                rect.xMax = savedxMax - buttonWidth - buttonSpace;
                rect.xMin = savedxMin + buttonWidth + buttonSpace;
                // GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
                var newValue = Widgets.TextField(rect, value.ToString());
                if (int.TryParse(newValue, out var newInteger))
                    value = (valMin <= newInteger && newInteger <= valMax) ? newInteger : value;
                else if (newValue == "") 
                        value = valMin;
            }


            Text.Anchor = savedAnchor;

            var key = name + "Explained";
            if (key.CanTranslate()) // The Explained description will only appear if it is available in XML.
            {
                Text.Font = GameFont.Tiny;
                listing.ColumnWidth -= 34;
                GUI.color = Color.gray;
                _ = listing.Label(key.Translate());
                listing.ColumnWidth += 34;
            }

            rect = listing.GetRect(0);
            rect.height = listing.CurHeight - startHeight;
            rect.y -= rect.height;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                if (!tooltip.NullOrEmpty())
                    TooltipHandler.TipRegion(rect, tooltip);
            }
        }

        public static void IntegerPlusMinus(this Listing_Standard listing, string name, ref int value, string tooltip = null, int valMin = 1, int valMax = 60)
        {
            bool checkOn = true;
            listing.IntegerPlusMinusCheckbox(name, ref value, ref checkOn, tooltip, valMin, valMax, checkBox : false);
        }

        public static void IntegerPlusMinusCheckbox(Rect rect, string name, ref int value, ref bool checkOn, string tooltip = null, int valMin = 1, int valMax = 60, bool checkBox = true, bool fieldOnlyIfCheck = true)
        {

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            var savedAnchor = Text.Anchor;

            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect, (name + "Title").Translate());

            rect.xMin += rect.width * 2 / 3; // I want a minus and plus button as well. Now we have the rect extremes.

            const float buttonWidth = 30f;
            const float buttonSpace = 4f;
            const float checkBoxSpace = 22f;
            float savedxMin = rect.xMin; float savedxMax = rect.xMax;
            Text.Anchor = TextAnchor.MiddleCenter; // GenUI.SetLabelAlign(TextAnchor.MiddleCenter);
            if (checkBox)
            {
                rect.xMin = savedxMin - checkBoxSpace - buttonSpace;
                rect.xMax = savedxMin - buttonSpace;
                Widgets.CheckboxLabeled(rect, (name + "Checkbox"), ref checkOn);
                rect.xMin = savedxMin; rect.xMax = savedxMax;
                
            }
            // else checkOn = true; // If checkBox is false, I still want to draw the - int + field. 

            if (!fieldOnlyIfCheck || checkOn)
            {
                rect.xMax = rect.xMin + buttonWidth;
                if (Widgets.ButtonText(rect, "-") && value > valMin)
                    value -= 1;

                rect.xMax = savedxMax;
                rect.xMin = rect.xMax - buttonWidth;
                if (Widgets.ButtonText(rect, "+") && value < valMax)
                    value += 1;

                Text.Anchor = TextAnchor.MiddleLeft;
                rect.xMax = savedxMax - buttonWidth - buttonSpace;
                rect.xMin = savedxMin + buttonWidth + buttonSpace;
                // GenUI.SetLabelAlign(TextAnchor.MiddleLeft);
                var newValue = Widgets.TextField(rect, value.ToString());
                if (int.TryParse(newValue, out var newInteger))
                    value = (valMin <= newInteger && newInteger <= valMax) ? newInteger : value;
                else if (newValue == "")
                    value = valMin;
            }


            Text.Anchor = savedAnchor;
        }

        public static void ValueLabeled<T>(this Listing_Standard listing, string name, ref T value, string tooltip = null)
        {
            var startHeight = listing.CurHeight;

            var rect = listing.GetRect("Hg".GetHeightCached() + listing.verticalSpacing);

            Text.Font = GameFont.Small;
            GUI.color = Color.white;

            var savedAnchor = Text.Anchor;

            Text.Anchor = TextAnchor.MiddleLeft;
            Widgets.Label(rect, (name + "Title").Translate());

            Text.Anchor = TextAnchor.MiddleRight;
            if (typeof(T).IsEnum)
                Widgets.Label(rect, (typeof(T).Name + "Option" + value.ToString()).Translate());
            else
                Widgets.Label(rect, value.ToString());

            Text.Anchor = savedAnchor;

            var key = name + "Explained";
            if (key.CanTranslate())
            {
                Text.Font = GameFont.Tiny;
                listing.ColumnWidth -= 34;
                GUI.color = Color.gray;
                _ = listing.Label(key.Translate());
                listing.ColumnWidth += 34;
            }

            rect = listing.GetRect(0);
            rect.height = listing.CurHeight - startHeight;
            rect.y -= rect.height;
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
                if (!tooltip.NullOrEmpty())
                    TooltipHandler.TipRegion(rect, tooltip);

                if (Event.current.isMouse && Event.current.button == 0 && Event.current.type == EventType.MouseDown)
                {
                    var keys = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
                    for (var i = 0; i < keys.Length; i++)
                    {
                        var newValue = keys[(i + 1) % keys.Length];
                        if (keys[i].ToString() == value.ToString())
                        {
                            value = newValue;
                            break;
                        }
                    }
                    Event.current.Use();
                }
            }
        }

        public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
        {
            if (middleAlignment || !label.NullOrEmpty())
                rect.y += Mathf.Round((rect.height - 16f) / 2f);
            if (!label.NullOrEmpty())
                rect.y += 5f;
            float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
            if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
            {
                TextAnchor anchor = Text.Anchor;
                GameFont font = Text.Font;
                Text.Font = GameFont.Tiny;
                float num2 = (label.NullOrEmpty() ? 18f : Text.CalcSize(label).y);
                rect.y = rect.y - num2 + 3f;
                if (!leftAlignedLabel.NullOrEmpty())
                {
                    Text.Anchor = TextAnchor.UpperLeft;
                    Widgets.Label(rect, leftAlignedLabel);
                }
                if (!rightAlignedLabel.NullOrEmpty())
                {
                    Text.Anchor = TextAnchor.UpperRight;
                    Widgets.Label(rect, rightAlignedLabel);
                }
                if (!label.NullOrEmpty())
                {
                    Text.Anchor = TextAnchor.UpperCenter;
                    Widgets.Label(rect, label);
                }
                Text.Anchor = anchor;
                Text.Font = font;
            }
            if (roundTo > 0f)
                num = Mathf.RoundToInt(num / roundTo) * roundTo;
            return num;
        }

        public static List<int> argsort(List<int> unsorted, out List<int> idx)
        {
            var tempSorted = unsorted
                .Select((x, i) => new KeyValuePair<int, int>(x, i))
                .OrderBy(x => x.Key)
                .ToList();

            idx = tempSorted.Select(x => x.Value).ToList();
            return tempSorted.Select(x => x.Key).ToList();
        }
        public static List<int> argsort(List<int> unsorted)
        {
            List<int> idx;
            idx = argsort(unsorted, out idx);
            return idx;
        }   

    }
}

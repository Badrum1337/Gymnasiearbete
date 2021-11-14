using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;
    public class DSSingleChoiceNode : DSNode
    {
        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);
            DialogueType = Enumerations.DSDialogueType.SingleChoice;
            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // OUTPUT CONTAINER

            foreach (DSChoiceSaveData choices in Choices)
            {
                Port choicePort = this.CreatePort(choices.Text);
                choicePort.userData = choices;
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

    }
}

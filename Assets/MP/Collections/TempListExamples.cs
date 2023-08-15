using UnityEngine;

#if UNITY_EDITOR
namespace MP.Collections.Examples
{
    using MP.Extensions;

    public class TempListExamples : MonoBehaviour
    {
        private void Start()
        {
            // Use the list to do temporary operations
            using(TempList<int> list = TempList.Claim<int>())
            {
                list.Add(10);
                list.Add(20);

                // You can use your extensions built for List<> on TempList<>
                list.Shuffle();
                
                using (TempList<int> list2 = TempList.Claim<int>())
                {
                    list2.Add(3);
                    list2.Add(4);
                }

                foreach (var number in list)
                {
                    Debug.Log(number);
                }

                // You can also nest them
                using (TempList<string> words = TempList.Claim<string>())
                {
                    words.Add("foo");
                    words.Add("bar");
                }
            }
        }
    }
}
#endif
using UnityEngine;

namespace BBQ.Common {
    public class InputGuard : MonoBehaviour {

        private int _semaphore;
        public static InputGuard I;

        void Awake() {
            if (I == null) I = this;
            _semaphore = 0;
        }

        public static void Lock() {
            I._semaphore++;
        }

        public static void UnLock() {
            I._semaphore--;
        }

        public static bool Guard() {
            return I._semaphore > 0;
        }

    }
}

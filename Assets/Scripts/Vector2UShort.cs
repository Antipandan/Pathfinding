
public struct Vector2UShort
{
        private ushort x;
        private ushort y;

        public Vector2UShort(ushort x, ushort y)
        {
                this.x = x;
                this.y = y;
        }

        public ushort X
        {
                get => x;
                set => x = value;
        }

        public ushort Y
        {
                get => y;
                set => y = value;
        }
}
        

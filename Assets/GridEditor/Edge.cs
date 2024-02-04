using System;

namespace GridEditor
{
    public class Edge : Identifiable, IEquatable<Edge>
    {
        public int NodeA;
        public int NodeB;
        public bool Equals(Edge other)
        {
            if (other == null) return false;
            return other.NodeA == NodeA && other.NodeB == NodeB || other.NodeA == NodeB && other.NodeB == NodeA;
        }
    }
}
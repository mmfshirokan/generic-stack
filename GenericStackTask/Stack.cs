using System;
using System.Collections;
using System.Collections.Generic;

namespace GenericStackTask
{
    /// <summary>
    /// Represents extendable last-in-first-out (LIFO) collection of the specified type T.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
    public class Stack<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;
        private int version;

        /// <summary>
        /// Initializes a new instance of the <see cref="Stack{T}"/> class that is empty and has the default initial capacity.
        /// </summary>
        public Stack()
        {
            this.count = 0;
            this.version = 0;

            this.items = Array.Empty<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stack{T}"/> class that is empty and has.
        /// the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements of stack.</param>
        public Stack(int capacity)
        {
            this.count = capacity;
            this.version = 0;

            this.items = new T[capacity];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stack{T}"/> class that contains elements copied.
        /// from the specified collection and has sufficient capacity to accommodate the
        /// number of elements copied.
        /// </summary>
        /// <param name="collection">The collection to copy elements from.</param>
        public Stack(IEnumerable<T>? collection)
        {
            _ = collection is null ? throw new ArgumentNullException(nameof(collection)) : collection;

            this.version = 0;
            this.items = new T[this.count];

            foreach (var item in collection)
            {
                this.Push(item);
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the stack.
        /// </summary>
        public int Count => this.count;

        /// <summary>
        /// Removes and returns the object at the top of the stack.
        /// </summary>
        /// <returns>The object removed from the top of the stack.</returns>
        public T Pop()
        {
            _ = this.items.Length == 0 ? throw new InvalidOperationException() : this.items;

            T temp = this.items[this.count - 1];

            this.items[this.count - 1] = default;

            this.count--;
            this.version++;

            return temp;
        }

        /// <summary>
        /// Returns the object at the top of the stack without removing it.
        /// </summary>
        /// <returns>The object at the top of the stack.</returns>
        public T Peek()
        {
            return this.items[this.Count - 1];
        }

        /// <summary>
        /// Inserts an object at the top of the stack.
        /// </summary>
        /// <param name="item">The object to push onto the stack.
        /// The value can be null for reference types.</param>
        public void Push(T item)
        {
            const int growthFactor = 2;
            this.count++;
            this.version++;

            if (this.count > this.items.Length)
            {
                Array.Resize(ref this.items, this.count * growthFactor);
            }

            this.items[this.count - 1] = item;
        }

       /// <summary>
       /// Copies the elements of stack to a new array.
       /// </summary>
       /// <returns>A new array containing copies of the elements of the stack.</returns>
        public T[] ToArray()
        {
            T[] newArray = new T[this.Count];
            Array.Copy(this.items, newArray, this.count);
            Array.Reverse(newArray);
            return newArray;
        }

        /// <summary>
        /// Determines whether an element is in the stack.
        /// </summary>
        /// <param name="item">The object to locate in the stack. The value can be null for reference types.</param>
        /// <returns>Return true if item is found in the stack; otherwise, false.</returns>
        public bool Contains(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            foreach (T it in this.items)
            {
                if (comparer.Equals(it, item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Removes all objects from the stack.
        /// </summary>
        public void Clear()
        {
            this.version = 0;
            this.count = 0;

            this.items = Array.Empty<T>();
        }

        /// <summary>
        /// Returns an enumerator for the stack.
        /// </summary>
        /// <returns>Return Enumerator object for the stack.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            int version = this.version;
            for (int i = this.count - 1; i >= 0; i--)
            {
                yield return this.items[i];
                _ = version == this.version ? version : throw new InvalidOperationException("Stack cannot be changed during iteration.");
            }
        }

        /// <summary>
        /// Returns an enumerator for the stack.
        /// </summary>
        /// <returns>Return Enumerator object for the stack.</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        // Add the necessary members to the class
    }
}

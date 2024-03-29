﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Network.Server
{
    /// <summary>Sent from server to client.</summary>
    public enum ServerPackets
    {
        welcome = 1,
        udpTest,
        playerInfo,
        playerNickname,
        playerReady,
        playerPosition,
        playerTeam,
        chatMessage,
        turnNumber,
        gameStage,
        battleground,
        cell,
        spawnHero,
        moveHero,
        availableCells,
        attackHero,
        heroValues
        //        playerPosition,
        //        playerReadiness,
        //        UPM
    }

    /// <summary>Sent from client to server.</summary>
    public enum ClientPackets
    {
        welcomeReceived = 1,
        udpTestReceived,
        playerInfoReceived,
        playerNicknameReceived,
        playerReadinessReceived,
        playerPositionReceived,
        playerTeamReceived,
        chatMessageReceived,
        moveHeroReceived,
        availableCellsReceived,
        attackHeroReceived,
    }

    /*
    //Universal Packet Manager Element, используется для того, чтобы различать одну информацию от другой. Может использоваться сервером и клиентом
    public enum UPM_Element
    {
        UPM_Int,
        UPM_String,
        UPM_Int2
    }

    //Universal Packet Manager, используется в качестве пакета собранного из UPM_Element в большой пакет
    public enum UPM
    {
        //Игрок - герой
        position = 0, // int[2] = {x, y} // Можно связать как с игрой, так и с лобби
        target, // int[2] = {x, y} Нужен для того, чтобы на клиенте просчитывать действия персонажа и отличать просто перемещение position от target
        readiness, // int (state) Игрок может быть готов (сделал ход / в лобби нажал кнопку), может быть не готов (не сделал ход // ничего не делает в лобби) 
        hero_health, // int
        hero_damage, // int
        hero_energy, // int
        hero_owner, // string
        hero_stats, // int[2] (position) int[2] (target) int (readiness) int (hero_health) int (hero_damage) int (hero_energy) string (hero_owner) (От сервера видимо отправляем всем клиентам, которые пускай сами думают, что делать с этой инфой)

        //Клетка
        cell_health, // int
        cell_damage, // int
        cell_energy, // int
        cellState, // int[2] = {x, y} +int (state) Для того, чтобы сервер посылал игроку доступные для него клетки в ходе

        //Состояние игры
        stage, // int (Какая сейчас стадия, лобби игра или еще что-то)
        turn, // int (чей ход по счету, можно использовать чисто для вывода на экран, а начислять игроку energy можно также с сервера)
        playersCount, // int
        playerNickName // string Хотя для того, чтобы получить playerNickname у игрока сначала необходимо знать, какой это игрок по счету (Как в пакете Welcome)
    }
    //Нужно сделать пакет который бы занимался спавном battlefield, чтобы он просто по порядку все клетки расшарил игроку, а тот просто переписао их на свой лад
    */
    public class Packet : IDisposable
    {
        private List<byte> buffer;
        private byte[] readableBuffer;
        private int readPos;

        /// <summary>Creates a new empty packet (without an ID).</summary>
        public Packet()
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0
        }

        /// <summary>Creates a new packet with a given ID. Used for sending.</summary>
        /// <param name="_id">The packet ID.</param>
        public Packet(int _id)
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            Write(_id); // Write packet id to the buffer
        }

        /// <summary>Creates a packet from which data can be read. Used for receiving.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public Packet(byte[] _data)
        {
            buffer = new List<byte>(); // Intitialize buffer
            readPos = 0; // Set readPos to 0

            SetBytes(_data);
        }

        #region Functions
        /// <summary>Sets the packet's content and prepares it to be read.</summary>
        /// <param name="_data">The bytes to add to the packet.</param>
        public void SetBytes(byte[] _data)
        {
            Write(_data);
            readableBuffer = buffer.ToArray();
        }

        /// <summary>Inserts the length of the packet's content at the start of the buffer.</summary>
        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count)); // Insert the byte length of the packet at the very beginning
        }

        /// <summary>Inserts the given int at the start of the buffer.</summary>
        /// <param name="_value">The int to insert.</param>
        public void InsertInt(int _value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(_value)); // Insert the int at the start of the buffer
        }

        /// <summary>Gets the packet's content in array form.</summary>
        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        /// <summary>Gets the length of the packet's content.</summary>
        public int Length()
        {
            return buffer.Count; // Return the length of buffer
        }

        /// <summary>Gets the length of the unread data contained in the packet.</summary>
        public int UnreadLength()
        {
            return Length() - readPos; // Return the remaining length (unread)
        }

        /// <summary>Resets the packet instance to allow it to be reused.</summary>
        /// <param name="_shouldReset">Whether or not to reset the packet.</param>
        public void Reset(bool _shouldReset = true)
        {
            if (_shouldReset)
            {
                buffer.Clear(); // Clear buffer
                readableBuffer = null;
                readPos = 0; // Reset readPos
            }
            else
            {
                readPos -= 4; // "Unread" the last read int
            }
        }
        #endregion

        #region Write Data
        /// <summary>Adds a byte to the packet.</summary>
        /// <param name="_value">The byte to add.</param>
        public void Write(byte _value)
        {
            buffer.Add(_value);
        }
        /// <summary>Adds an array of bytes to the packet.</summary>
        /// <param name="_value">The byte array to add.</param>
        public void Write(byte[] _value)
        {
            buffer.AddRange(_value);
        }
        /// <summary>Adds a short to the packet.</summary>
        /// <param name="_value">The short to add.</param>
        public void Write(short _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds an int to the packet.</summary>
        /// <param name="_value">The int to add.</param>
        public void Write(int _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }

        public void Write(CellValues[,] _value)
        {
            Write(_value.GetLength(0));
            Write(_value.GetLength(1));
            for (int j = 0; j < _value.GetLength(1); j++)
            {
                for (int i = 0; i < _value.GetLength(0); i++)
                {
                    Write(_value[i, j]);
                }
            }
        }

        public void Write(CellValues _value)
        {
            Write(_value.locationName);
            Write(_value.damagePerTurn);
            Write(_value.healthPerTurn);
            Write(_value.energyPerTurn);
            Write(_value.position);
            Write(_value.heroId);
        }

        public void Write(HeroValues _value)
        {
            Write(_value.ID);
            Write(_value.position);
            Write(_value.owner);
            Write(_value.team);

            Write(_value.defaultHealth);
            Write(_value.health);

            Write(_value.defaultDamage);
            Write(_value.damage);

            Write(_value.defaultEnergy);
            Write(_value.energy);
        }

        public void Write(Vector2 _value)
        {
            Write(_value.x);
            Write(_value.y);
        }

        public void Write(Vector2[] _value)
        {
            int _arrayLength = _value.Length;
            Write(_arrayLength);
            for (int i = 0; i < _arrayLength; i++)
            {
                Write(_value[i]);
            }
        }

        public void Write(Vector2Int _value)
        {
            Write(_value.x);
            Write(_value.y);
        }

        public void Write(Vector2Int[] _value)
        {
            int _arrayLength = _value.Length;
            Write(_arrayLength);
            for (int i = 0; i < _arrayLength; i++)
            {
                Write(_value[i]);
            }
        }

        /// <summary>Adds a long to the packet.</summary>
        /// <param name="_value">The long to add.</param>
        public void Write(long _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a float to the packet.</summary>
        /// <param name="_value">The float to add.</param>
        public void Write(float _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /// <summary>Adds a bool to the packet.</summary>
        /// <param name="_value">The bool to add.</param>
        public void Write(bool _value)
        {
            buffer.AddRange(BitConverter.GetBytes(_value));
        }
        /* **New**
        /// <summary>Adds a bool array to the packet.</summary>
        /// <param name="_values">The bool array to add.</param>
        public void Write(bool[] _values)
        {
            Write(_values.Length);
            for (int i = 0; i < _values.Length; i++)
            {
                Write(_values[i]);
            }
        }
        /// <summary>Adds a string to the packet.</summary>
        /// <param name="_value">The string to add.</param>
        */
        public void Write(string _value)
        {
            Write(_value.Length); // Add the length of the string to the packet
            buffer.AddRange(Encoding.ASCII.GetBytes(_value)); // Add the string itself
        }
        #endregion

        #region Read Data
        /// <summary>Reads a byte from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte ReadByte(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte _value = readableBuffer[readPos]; // Get the byte at readPos' position
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the byte
            }
            else
            {
                throw new Exception("Could not read value of type 'byte'!");
            }
        }

        /// <summary>Reads an array of bytes from the packet.</summary>
        /// <param name="_length">The length of the byte array.</param>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public byte[] ReadBytes(int _length, bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                byte[] _value = buffer.GetRange(readPos, _length).ToArray(); // Get the bytes at readPos' position with a range of _length
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += _length; // Increase readPos by _length
                }
                return _value; // Return the bytes
            }
            else
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }
        }

        /// <summary>Reads a short from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public short ReadShort(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                short _value = BitConverter.ToInt16(readableBuffer, readPos); // Convert the bytes to a short
                if (_moveReadPos)
                {
                    // If _moveReadPos is true and there are unread bytes
                    readPos += 2; // Increase readPos by 2
                }
                return _value; // Return the short
            }
            else
            {
                throw new Exception("Could not read value of type 'short'!");
            }
        }

        /// <summary>Reads an int from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public int ReadInt(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                int _value = BitConverter.ToInt32(readableBuffer, readPos); // Convert the bytes to an int
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the int
            }
            else
            {
                throw new Exception("Could not read value of type 'int'!");
            }
        }

        public CellValues[,] ReadCellValuesArray(bool _moveReadPos = true)
        {
            int _rows = ReadInt();
            int _cols = ReadInt();
            CellValues[,] _battlefield = new CellValues[_rows, _cols];
            for (int j = 0; j < _cols; j++)
            {
                for (int i = 0; i < _rows; i++)
                {
                    _battlefield[i, j] = ReadCellValues();
                }
            }
            return _battlefield;
        }

        public CellValues ReadCellValues(bool _moveReadPos = true)
        {
            return new CellValues(ReadString(_moveReadPos), ReadInt(_moveReadPos), ReadInt(_moveReadPos), ReadInt(_moveReadPos), ReadVector2Int(_moveReadPos), ReadInt(_moveReadPos));
        }

        public HeroValues ReadHeroValues(bool _moveReadPos = true)
        {
            return new HeroValues(ReadInt(), ReadVector2Int(), ReadString(), ReadInt(), ReadInt(), ReadInt(), ReadInt(), ReadInt(), ReadInt(), ReadInt());
        }

        public Vector2[] ReadVector2Array(bool _moveReadPos = true)
        {
            int _length = ReadInt();
            Vector2[] _vector2Array = new Vector2[_length];
            for (int i = 0; i < _length; i++)
            {
                _vector2Array[i] = ReadVector2();
            }
            return _vector2Array;
        }

        public Vector2 ReadVector2(bool _moveReadPos = true)
        {
            return new Vector2(ReadFloat(_moveReadPos), ReadFloat(_moveReadPos));
        }

        public Vector2Int[] ReadVector2IntArray(bool _moveReadPos = true)
        {
            int _length = ReadInt();
            Vector2Int[] _vector2IntArray = new Vector2Int[_length];
            for (int i = 0; i < _length; i++)
            {
                _vector2IntArray[i] = ReadVector2Int();
            }
            return _vector2IntArray;
        }

        public Vector2Int ReadVector2Int(bool _moveReadPos = true)
        {
            return new Vector2Int(ReadInt(_moveReadPos), ReadInt(_moveReadPos));
        }

        /// <summary>Reads a long from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public long ReadLong(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                long _value = BitConverter.ToInt64(readableBuffer, readPos); // Convert the bytes to a long
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 8; // Increase readPos by 8
                }
                return _value; // Return the long
            }
            else
            {
                throw new Exception("Could not read value of type 'long'!");
            }
        }

        /// <summary>Reads a float from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public float ReadFloat(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                float _value = BitConverter.ToSingle(readableBuffer, readPos); // Convert the bytes to a float
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 4; // Increase readPos by 4
                }
                return _value; // Return the float
            }
            else
            {
                throw new Exception("Could not read value of type 'float'!");
            }
        }

        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool ReadBool(bool _moveReadPos = true)
        {
            if (buffer.Count > readPos)
            {
                // If there are unread bytes
                bool _value = BitConverter.ToBoolean(readableBuffer, readPos); // Convert the bytes to a bool
                if (_moveReadPos)
                {
                    // If _moveReadPos is true
                    readPos += 1; // Increase readPos by 1
                }
                return _value; // Return the bool
            }
            else
            {
                throw new Exception("Could not read value of type 'bool'!");
            }
        }
        /* **New**
        /// <summary>Reads a bool from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public bool[] ReadBoolArray(bool _moveReadPos = true)
        {
            int _length = ReadInt();
            bool[] _values = new bool[_length];
            for (int i = 0; i < _length; i++)
            {
                _values[i] = ReadBool();
            }
            return _values;
        }
        */
        /// <summary>Reads a string from the packet.</summary>
        /// <param name="_moveReadPos">Whether or not to move the buffer's read position.</param>
        public string ReadString(bool _moveReadPos = true)
        {
            try
            {
                int _length = ReadInt(); // Get the length of the string
                string _value = Encoding.ASCII.GetString(readableBuffer, readPos, _length); // Convert the bytes to a string
                if (_moveReadPos && _value.Length > 0)
                {
                    // If _moveReadPos is true string is not empty
                    readPos += _length; // Increase readPos by the length of the string
                }
                return _value; // Return the string
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }
        #endregion

        private bool disposed = false;

        protected virtual void Dispose(bool _disposing)
        {
            if (!disposed)
            {
                if (_disposing)
                {
                    buffer = null;
                    readableBuffer = null;
                    readPos = 0;
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

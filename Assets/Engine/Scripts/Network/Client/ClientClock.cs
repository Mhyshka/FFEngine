using System;
using System.Collections.Generic;

using FF.Network.Message;

namespace FF.Network
{
    internal class ClientClock
    {
        static int MAX_LATENCY_VALUES = 10;
        static int CLOCK_SYNC_VALUES = 50;

        #region Callback
        internal FFFloatClientCallback onLatencyUpdate = null;
        #endregion

        #region Properties
        protected FFNetworkClient _client;
        #endregion

        internal ClientClock(FFNetworkClient a_client)
        {
            _client = a_client;
            _latencyResults = new Queue<float>();
            _recordedClockOffsets = new List<long>(CLOCK_SYNC_VALUES);
        }

        internal void ResyncNow()
        {
            _latencyResults = new Queue<float>();
            _recordedClockOffsets = new List<long>(CLOCK_SYNC_VALUES);
            _isClockSynced = false;

            _averageLatency = -1;
            if (onLatencyUpdate != null)
                onLatencyUpdate(_client, _averageLatency);
        }

        #region Latency
        #region Properties
        protected Queue<float> _latencyResults = null;

        protected float _averageLatency = 0f;
        internal float Latency
        {
            get
            {
                return _averageLatency;
            }
        }
        #endregion
        
        protected void NewLatencyValue(double a_value)
        {
            _latencyResults.Enqueue((float)a_value);

            if (_latencyResults.Count > MAX_LATENCY_VALUES)
            {
                _latencyResults.Dequeue();
            }

            ComputeAverageLatency();

            if (onLatencyUpdate != null)
                onLatencyUpdate(_client, _averageLatency);
        }

        protected void ComputeAverageLatency()
        {
            _averageLatency = 0f;
            foreach (float a_val in _latencyResults)
            {
                _averageLatency += a_val;
            }

            if (_latencyResults.Count > 0)
            {
                _averageLatency /= (float)(_latencyResults.Count);
            }
        }
        #endregion

        #region Timeout
        protected long _lastMessageReceivedTicks = 0L;
        protected static int TIMEOUT_DURATION = 2000;
        protected static int CONNECTING_TIMEOUT_DURATION = 10000;
        internal void NewMessageReceived()
        {
            _lastMessageReceivedTicks = DateTime.Now.Ticks;
        }

        internal bool IsTimedout
        {
            get
            {
                TimeSpan span = TimeSpan.FromTicks(DateTime.Now.Ticks - _lastMessageReceivedTicks);
                if (_client.IsReady)
                    return span.TotalMilliseconds > TIMEOUT_DURATION;
                else
                    return span.TotalMilliseconds > CONNECTING_TIMEOUT_DURATION;
            }
        }
        #endregion

        #region Clock Sync
        #region Properties
        protected bool _isClockSynced = false;
        internal bool IsClockSynced
        {
            get
            {
                return _isClockSynced;
            }
        }

        protected TimeSpan _clockOffset;
        internal TimeSpan ClockOffset
        {
            get
            {
                return _clockOffset;
            }
        }

        /// <summary>
        /// Time between heartbeat during synchronization phase.
        /// </summary>
        protected double _clockSyncTimespan = 50d;

        /// <summary>
        /// Ticks
        /// </summary>
        protected List<long> _recordedClockOffsets;
        #endregion


        internal TimeSpan TimeOffset(long a_remoteTimestamp)
        {
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - ConvertRemoteToLocalTime(a_remoteTimestamp));
            return span;
        }

        internal long ConvertRemoteToLocalTime(long a_remoteTimestamp)
        {
            long localTime = 0L;
            localTime = a_remoteTimestamp + _clockOffset.Ticks;
            return localTime;
        }

        internal long ConvertLocalToRemoteTime(long a_localTimestamp)
        {
            long remoteTime = 0L;
            remoteTime = a_localTimestamp - _clockOffset.Ticks;
            return remoteTime;
        }

        protected void ComputeClockOffset()
        {
            _isClockSynced = true;

            long average = 0L;
            foreach (long each in _recordedClockOffsets)
            {
                average += each;
            }
            average /= _recordedClockOffsets.Count;

            long averageDistance = 0L;
            foreach (long each in _recordedClockOffsets)
            {
                long distance = average - each;
                if (distance < 0)
                    distance = -distance;

                averageDistance += distance;
            }
            averageDistance /= _recordedClockOffsets.Count;

            long centeredAverage = 0L;
            int usedValue = 0;
            foreach (long each in _recordedClockOffsets)
            {
                long distance = average - each;
                if (distance < 0)
                    distance = -distance;

                if (distance <= averageDistance * 2L)
                {
                    usedValue++;
                    centeredAverage += each;
                }
            }
            centeredAverage /= usedValue;
            _clockOffset = new TimeSpan(centeredAverage);
            FFLog.Log(EDbgCat.ClientClock, "Clock offset : " + _clockOffset.TotalSeconds.ToString(".000"));
        }
        #endregion

        #region Heartbeat
        protected double _heartbeatTimespan = 500d;//in MS
        protected DateTime _lastHeartbeatTimestamp;

        internal void SetConnectionTime(DateTime a_connectionTime)
        {
            _lastHeartbeatTimestamp = a_connectionTime;
            NewMessageReceived();
        }

        internal void UpdateHeartbeat()
        {
            TimeSpan span = DateTime.Now - _lastHeartbeatTimestamp;
            if (span.TotalMilliseconds > (_isClockSynced ? _heartbeatTimespan : _clockSyncTimespan))
            {
                SentRequest request = new SentRequest(new MessageEmptyData(),
                                                        EMessageChannel.Heartbeat.ToString(),
                                                        Engine.Network.NextRequestId,
                                                        1f,
                                                        false);
                request.onSucces += OnHeartbeatSuccessReceived;
                _client.QueueRequest(request);
                _lastHeartbeatTimestamp = DateTime.Now;

                FFLog.Log(EDbgCat.ClientClock, "Heartbeat queued.");
            }
        }

        protected void OnHeartbeatSuccessReceived(ReadResponse a_heartbeat)
        {
            MessageLongData data = a_heartbeat.Data as MessageLongData;
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - data.Data);
            double latency = span.TotalMilliseconds;

            NewLatencyValue(latency);

            if (!_isClockSynced)
            {
                long clockOffset = DateTime.Now.Ticks - a_heartbeat.Timestamp - (span.Ticks / 2);
                _recordedClockOffsets.Add(clockOffset);

                if (_recordedClockOffsets.Count == CLOCK_SYNC_VALUES)
                {
                    ComputeClockOffset();
                }
            }
            /*else
            {
                span = TimeOffset(a_heartbeat.Timestamp);
                FFLog.LogError("Offset : " + span.TotalMilliseconds.ToString());
            }*/

            FFLog.Log(EDbgCat.ClientClock, "On Heartbeat received : " + latency.ToString());
        }
        #endregion
    }
}

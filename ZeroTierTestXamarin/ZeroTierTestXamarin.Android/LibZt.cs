using System;
using System.Runtime.InteropServices;

public class LibZt
{
            public enum SockAddrFamily
        {
            Inet = 2,
            Inet6 = 10
        }

        public enum SockType
        {
            Stream = 1,
            Datagram = 2
        }

        public enum FCNTL_CMDS
        {
            F_GETFL = 3,
            F_SETFL = 4
        }

        public enum FILE_ACCESS_MODES
        {
            O_NONBLOCK = 1,
            O_NDELAY = 1,
            O_RDONLY = 2,
            O_WRONLY = 4,
            O_RDWR = 6
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TIMEVAL
        {
            public int tv_sec;
            public int tv_usec;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FDSET
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] fd_bits;
        }

        //#define FD_SET(n, p) ((p)->fd_bits[(n)/8] |= (1 << ((n) & 7)))
        public static void FD_SET(int fd, ref FDSET set)
        {
            int index = fd / 8;
            byte mask = (byte)(1 << (fd & 7));
            var newValue = set.fd_bits[index] | mask;
            set.fd_bits[index] = (byte)newValue;
        }

        //#define FD_CLR(n, p) ((p)->fd_bits[(n)/8] &= ~(1 << ((n) & 7)))
        public static void FD_CLR(int fd, ref FDSET set)
        {
            int index = fd / 8;
            byte mask = (byte)(1 << (fd & 7));
            var newValue = set.fd_bits[index] & ~mask;
            set.fd_bits[index] = (byte)newValue;
        }

        //#define FD_ISSET(n,p) ((p)->fd_bits[(n)/8] & (1 << ((n) & 7)))
        public static int FD_ISSET(int fd, ref FDSET set)
        {
            int index = fd / 8;
            byte mask = (byte)(1 << (fd & 7));
            var rv = set.fd_bits[index] & mask;

            return rv;
        }

        //#define FD_ZERO(p) memset((void*)(p),0,sizeof(*(p)))
        public static void FD_ZERO(ref FDSET set)
        {
            if (set.fd_bits == null)
                set.fd_bits = new byte[4];
            else
                Array.Clear(set.fd_bits, 0, set.fd_bits.Length);
        }



    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern long zts_get_peer_count();

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern void zts_stop();

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_has_address(ulong nwid);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_get_address(ulong nwid, IntPtr addr, int address_family);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_socket(int socket_family, int socket_type, int protocol);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_close(int fd);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_bind(int fd, IntPtr addr, int addrlen);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_listen(int fd, int backlog);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_fcntl(int fd, int cmd, int flags);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_accept(int fd, IntPtr addr, int addrlen);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_select(int nfds, [In,Out] ref FDSET readfds, [In, Out] ref FDSET writefds, [In, Out] ref FDSET exceptfds, ref TIMEVAL timeout);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_read(int fd, byte[] buf, int len);

    [DllImport("zt", CallingConvention = CallingConvention.Cdecl)]
    public static extern int zts_get_num_assigned_addresses(ulong nwid);
}
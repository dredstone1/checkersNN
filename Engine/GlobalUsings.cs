#if BOARD_10
global using BoardBits = System.UInt128;
#elif BOARD_8
global using BoardBits = ulong;
#else
#error Unsupported board size
#endif

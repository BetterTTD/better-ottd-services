module Domain

type ServerName = string

type Server =
    { Name   : ServerName
      Online : bool }

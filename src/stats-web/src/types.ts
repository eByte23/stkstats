import { type } from "os"


export type Teams = { [teamId: string]: Team }

export type Team = {
    ShortId: string
    TeamId: string
    TeamName: string
    SeasonName: string
    SeasonId: string
    Record: string
    Games: {
        Item1: string,
        Item2: string
    }[]
}

export type RecentGames = Game[]

export type Game = {
    GameId: string,
    GameChangerGameId: string,

    TeamUnqiueId: string,
    TeamShortId: string,
    TeamId: string,
    TeamName: string,

    Grade: string,
    HomeAway: "Home" | "Away",

    OppositionName: string,
    OppositionId: string,

    SeasonName: string,
    SeasonId: string,

    Location: string,
    Date: string,
    Result: "W" | "L" | "D",

    HomeRuns: number,
    AwayRuns: number,

    Players: Player[],
    GameUrl: string,
    GameShortId: string,
}


export type Player = {
    ShortId: string
    UniqueId: string
    Name: string
    GamesPlayed?: number
    GameChangerIds: string[],
    Hitting: BasicHitting,
}

export type BasicHitting = {
    PA: string
    AB: string
    H: string
    TB: string
    Singles: string
    Doubles: string
    Triples: string
    HR: string
    RBI: string
    R: string
    BB: string
    SO: string
    SF: string
    HBP: string
    SOL: string
    AVG: string
    SLG: string
    OPS: string
    OBP: string
}

export type IndividualPlayer = {
    FullName: string
    ShortId: string
    PlayerId: string
    ReferencePlayerIds: string[]
    GamesPlayed: Game[]
    SeasonTotals: SeasonTotal[]
    TotalHitting: BasicHitting
}

export type SeasonTotal =
    {
        SeasonId: string,
        SeasonName: string,
        GamesPlayed: number,
        Hitting: BasicHitting,
    }




export type PlayerShortIds = string[]
export type GameShortIds = string[]

export type TeamViewModel = {
    TeamId: string,
    ReferenceTeamId: string,
    TeamName: string,
    TeamShortId: string,
    Grade: null,
    SeasonId: string,
    SeasonName: string,
    Games: TeamGamePlayer[]
    Players: Player[],
    TeamTotalHitting: {}
}


export type TeamGamePlayer = {
    GameId: string,
    ShortId: string,
    Name: string,
    Date: string,
    Hittin: string,
    OppositionTeam: string,
    HomeRuns: number,
    AwayRuns: number,
    Result: string,
}
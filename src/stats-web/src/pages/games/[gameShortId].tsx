import { GetStaticProps } from "next"
import fs from "fs"
import { Game, Teams } from "@/types"
import Link from "next/link"

type GamePageProps = {
    game: Game
}

type BoxScoreShortT = {
    runs: number
    hits: number
    errors: number
    teamName: string
    teamShortId: string
}

const getTeamShortBoxScore = (game: Game, homeAway: "Home" | "Away"): BoxScoreShortT => {

    return {
        runs: homeAway === "Home" ? game.HomeRuns : game.AwayRuns,
        hits: 0,
        errors: 0,
        teamName: game.HomeAway === homeAway ? game.TeamName : game.OppositionName,
        teamShortId: game.HomeAway === homeAway ? game.TeamShortId : ""
    }

}

const getWinLossText = (game: Game) => {
    switch (game.Result) {
        case "W":
            return (<span className="text-green-500">Win</span>)
        case "L":
            return (<span className="text-red-500">Loss</span>)
        case "D":
            return (<span className="text-yellow-500">Draw</span>)
    }

}

const GamePage = ({ game }: GamePageProps) => {

    if (!game) {
        return <div>Game not found</div>
    }

    const homeTeamShortBoxScore = getTeamShortBoxScore(game, "Home")
    const awayTeamShortBoxScore = getTeamShortBoxScore(game, "Away")

    return (
        <div>

            <div>
                <h1 className="text-2xl font-semibold">
                    <Link href={`/teams/${game?.TeamId}`} className="text-2xl hover:underline hover:text-blue-500">{game.TeamName}</Link> vs. {game.OppositionName}
                </h1>
                <h3 className="text-md font-semibold">{getWinLossText(game)}</h3>
            </div>

            <div className="pb-4">
                <BoxScoreShort away={awayTeamShortBoxScore} home={homeTeamShortBoxScore} />
            </div>

            <div>
                <h2 className="text-xl">Roster</h2>
                <table className="table-auto">
                    <thead>
                        <tr>
                            <th className="px-2 py-1 text-left">Name</th>
                            <th className="px-2 py-1">PA</th>
                            <th className="px-2 py-1">AB</th>
                            <th className="px-2 py-1">H</th>
                            <th className="px-2 py-1">TB</th>
                            <th className="px-2 py-1">Singles</th>
                            <th className="px-2 py-1">Doubles</th>
                            <th className="px-2 py-1">Triples</th>
                            <th className="px-2 py-1">HR</th>
                            <th className="px-2 py-1">RBI</th>
                            <th className="px-2 py-1">R</th>
                            <th className="px-2 py-1">BB</th>
                            <th className="px-2 py-1">SO</th>
                            <th className="px-2 py-1">SOL</th>
                            <th className="px-2 py-1">AVG</th>
                            <th className="px-2 py-1">SLG</th>
                            <th className="px-2 py-1">OPS</th>
                            <th className="px-2 py-1">OBP</th>
                        </tr>
                    </thead>

                    <tbody>
                        {game.Players?.map((player) => (
                            <tr key={player.UniqueId}>
                                <td className="border px-4 py-2">
                                    <Link href={`/players/${player.ShortId}`} className="underline hover:text-blue-500">
                                        {player.Name}
                                    </Link>
                                </td>
                                <td className="border px-2 py-1">{player.Hitting.PA}</td>
                                <td className="border px-2 py-1">{player.Hitting.AB}</td>
                                <td className="border px-2 py-1">{player.Hitting.H}</td>
                                <td className="border px-2 py-1">{player.Hitting.TB}</td>
                                <td className="border px-2 py-1">{player.Hitting.Singles}</td>
                                <td className="border px-2 py-1">{player.Hitting.Doubles}</td>
                                <td className="border px-2 py-1">{player.Hitting.Triples}</td>
                                <td className="border px-2 py-1">{player.Hitting.HR}</td>
                                <td className="border px-2 py-1">{player.Hitting.RBI}</td>
                                <td className="border px-2 py-1">{player.Hitting.R}</td>
                                <td className="border px-2 py-1">{player.Hitting.BB}</td>
                                <td className="border px-2 py-1">{player.Hitting.SO}</td>
                                <td className="border px-2 py-1">{player.Hitting.SOL}</td>
                                <td className="border px-2 py-1">{player.Hitting.AVG}</td>
                                <td className="border px-2 py-1">{player.Hitting.SLG}</td>
                                <td className="border px-2 py-1">{player.Hitting.OPS}</td>
                                <td className="border px-2 py-1">{player.Hitting.OBP}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default GamePage

export const getStaticProps: GetStaticProps<GamePageProps, { gameShortId?: string }> = async ({ params }) => {

    const gameShortId = params?.["gameShortId"]
    console.log("getStaticProps", { shortId: gameShortId })

    const content = fs.readFileSync(process.cwd() + `/data/game-output/${gameShortId}.json`).toString()

    const game = JSON.parse(content) as Game || []

    return {
        props: {
            game: game || {},
        },
    }
}

export const getStaticPaths = async () => {
    const content = fs.readFileSync(process.cwd() + "/data/recent-games.json").toString()

    const teams = JSON.parse(content) as Teams || {}

    const paths = Object.keys(teams).map((teamId) => {
        const team = teams[teamId]
        return team.Games?.map((game) => game.Item1) || []
    })
        .flat()
        .map((gameShortId) => ({ params: { gameShortId: gameShortId } }))

    return {
        paths: paths,
        fallback: false,
    }
}



const BoxScoreShort = ({ away, home }: { away: BoxScoreShortT, home: BoxScoreShortT }) => {
    return <table className="">
        <thead>
            <tr>
                <th className="px-4 py-2 text-left">Team</th>
                <th className="px-4 py-2">Runs</th>
                <th className="px-4 py-2">Hits</th>
                <th className="px-4 py-2">Errors</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td className="border px-4 py-2 font-semibold">
                    {away.teamShortId !== "" ? (
                        <Link href={`/teams/${away.teamShortId}`} className="underline hover:text-blue-500">
                            {away.teamName}
                        </Link>
                    ) : (away.teamName)}

                </td>
                <td className="border px-4 py-2">{away.runs}</td>
                <td className="border px-4 py-2">{away.hits}</td>
                <td className="border px-4 py-2">{away.errors}</td>
            </tr>
            <tr>
                <td className="border px-4 py-2 font-semibold">
                    {home.teamShortId !== "" ? (
                        <Link href={`/teams/${home.teamShortId}`} className="underline hover:text-blue-500">
                            {home.teamName}
                        </Link>
                    ) : (home.teamName)}
                </td>
                <td className="border px-4 py-2">{home.runs}</td>
                <td className="border px-4 py-2">{home.hits}</td>
                <td className="border px-4 py-2">{home.errors}</td>
            </tr>
        </tbody>
    </table>
}


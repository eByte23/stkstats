import { GetStaticProps } from "next"
import fs from "fs"
import { Game, GameShortIds } from "@/types"
import Link from "next/link"
import { HittingStatsTable } from "@/components/HittingStatsTable"

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
                <HittingStatsTable players={game.Players} showNameColumn={true} />
            </div>
        </div>
    )
}

// export const runtime ='experimental-edge';
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
    const content = fs.readFileSync(process.cwd() + "/data/games.json").toString()

    const games = JSON.parse(content) as GameShortIds || []

    const paths = games
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



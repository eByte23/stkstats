import { BasicHitting, Player } from "@/types"
import Link from "next/link"

type BasicHittingWithGamesPlayed = BasicHitting & { GamesPlayed: number }

type TeamPlayerStats = {
    PlayerId: string,
    Name: string,
    ShortId: string,
    Hitting: BasicHittingWithGamesPlayed
}

type HittingStatsTableProps = { players: TeamPlayerStats[], teamTotals: BasicHittingWithGamesPlayed }

export const TeamHittingStatsTable = ({ players }: HittingStatsTableProps) => {
    return <table className="table-auto">
        <thead>
            <tr>
                <th className="px-3 text-left">Name</th>
                <th className="">GP</th>
                <th className="">PA</th>
                <th className="">AB</th>
                <th className="">H</th>
                <th className="">TB</th>
                <th className="">1B</th>
                <th className="">2B</th>
                <th className="">3B</th>
                <th className="">HR</th>
                <th className="">RBI</th>
                <th className="">R</th>
                <th className="">BB</th>
                <th className="">SO</th>
                <th className="">HBP</th>
                <th className="">SOL</th>
                <th className="">AVG</th>
                <th className="">SLG</th>
                <th className="">OPS</th>
                <th className="">OBP</th>
            </tr>
        </thead>

        <tbody>
            {players?.map((player) => (
                <tr key={player.PlayerId}>
                    <td className="border px-3">
                        <Link href={`/players/${player.ShortId}`} className="underline hover:text-blue-500">
                            {player.Name}
                        </Link>
                    </td>
                    <td className="border px-3 text-center">{player.Hitting.GamesPlayed}</td>
                    <td className="border px-3 text-center">{player.Hitting.PA}</td>
                    <td className="border px-3 text-center">{player.Hitting.AB}</td>
                    <td className="border px-3 text-center">{player.Hitting.H}</td>
                    <td className="border px-3 text-center">{player.Hitting.TB}</td>
                    <td className="border px-3 text-center">{player.Hitting.Singles}</td>
                    <td className="border px-3 text-center">{player.Hitting.Doubles}</td>
                    <td className="border px-3 text-center">{player.Hitting.Triples}</td>
                    <td className="border px-3 text-center">{player.Hitting.HR}</td>
                    <td className="border px-3 text-center">{player.Hitting.RBI}</td>
                    <td className="border px-3 text-center">{player.Hitting.R}</td>
                    <td className="border px-3 text-center">{player.Hitting.BB}</td>
                    <td className="border px-3 text-center">{player.Hitting.SO}</td>
                    <td className="border px-3 text-center">{player.Hitting.HBP}</td>
                    <td className="border px-3 text-center">{player.Hitting.SOL}</td>
                    <td className="border px-3 text-center">{player.Hitting.AVG}</td>
                    <td className="border px-3 text-center">{player.Hitting.SLG}</td>
                    <td className="border px-3 text-center">{player.Hitting.OPS}</td>
                    <td className="border px-3 text-center">{player.Hitting.OBP}</td>
                </tr>
            ))}
        </tbody>
    </table>
}
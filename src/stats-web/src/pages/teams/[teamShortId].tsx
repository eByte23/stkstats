import { GetStaticProps } from "next"
import fs from "fs"
import { TeamViewModel, Teams } from "@/types"
import { TeamHittingStatsTable } from "@/components/TeamHittingStatsTable"

type TeamPageProps = {
    team: TeamViewModel
}

const TeamPage = (props: TeamPageProps) => {

    return (
        <div className="flex flex-col justify-center items-center mx-auto ">
            <h1 className="text-3xl">{props.team.TeamName}</h1>

            <div>
                <div className="border-b border-gray-200 dark:border-gray-700">
                    <nav className="flex space-x-2" aria-label="Tabs" role="tablist">
                        <button type="button"
                            className="hs-tab-active:font-semibold hs-tab-active:border-blue-600 hs-tab-active:dark:border-grey-300 py-4 px-1 inline-flex items-center gap-2 border-b-[3px] border-transparent 
                        text-sm whitespace-nowrap text-gray-500 hover:text-blue-600 active" id="tabs-with-underline-item-1" data-hs-tab="#tabs-with-underline-1" aria-controls="tabs-with-underline-1" role="tab">
                            All Games
                        </button>
                        <button type="button" className="hs-tab-active:font-semibold hs-tab-active:border-blue-600 hs-tab-active:dark:border-grey-300 py-4 px-1 inline-flex items-center gap-2 border-b-[3px] border-transparent text-sm whitespace-nowrap text-gray-500 hover:text-blue-600" id="tabs-with-underline-item-2" data-hs-tab="#tabs-with-underline-2" aria-controls="tabs-with-underline-2" role="tab">
                            Games
                        </button>
                    </nav>
                </div>

                <div className="mt-3">
                    <div id="tabs-with-underline-1" role="tabpanel" aria-labelledby="tabs-with-underline-item-1">
                        <TeamHittingStatsTable players={props.team.Players.map((p) => ({
                            PlayerId: p.UniqueId,
                            Name: p.Name,
                            ShortId: p.ShortId,
                            Hitting: ({...p.Hitting, GamesPlayed: p.GamesPlayed!}),
                        }))}
                        teamTotals={({}) as any}
                        />

                    </div>
                    <div id="tabs-with-underline-2" className="hidden" role="tabpanel" aria-labelledby="tabs-with-underline-item-2">
                       <table>
                            <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Opponent</th>
                                    <th>Result</th>
                                    <th>Runs</th>
                                    <th>Hits</th>
                                    <th>Errors</th>
                                </tr>
                            </thead>
                            <tbody>
                                {props.team.Games.map((g) => (
                                    <tr key={g.GameId}>
                                        <td>{g.Date}</td>
                                        <td>{g.OppositionTeam}</td>
                                        <td>{g.Result}</td>
                                        <td>{g.HomeRuns} - {g.AwayRuns}</td>
                                        <td>{`0`}</td>
                                        <td>{`0`}</td>
                                    </tr>
                                ))}
                            </tbody>
                       </table>
                    </div>
                </div>
            </div>
        </div>
    )
}



// export const runtime ='experimental-edge';
export default TeamPage



export const getStaticProps: GetStaticProps<TeamPageProps, { teamShortId?: string }> = async ({ params }) => {

    const teamShortId = params?.["teamShortId"]
    console.log(`teamid`, {
        teamShortId
    })

    const content = fs.readFileSync(process.cwd() + `/data/team-output/${teamShortId}.json`).toString()

    const team = JSON.parse(content) as TeamViewModel || []

    return {
        props: {
            team: team || {},
        },
    }
}

export const getStaticPaths = async () => {
    const content = fs.readFileSync(process.cwd() + "/data/teams.json").toString()

    const teams = JSON.parse(content) as Teams || {};

    // console.log(teams);

    const paths = Object.keys(teams)
        .map((teamShortId) => ({ params: { teamShortId: teams[teamShortId].ShortId } }))

    return {
        paths: paths,
        fallback: false,
    }
}


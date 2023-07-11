import { GetStaticProps } from "next"
import fs from "fs"
import { IndividualPlayer, Player, PlayerShortIds } from "@/types"
import { HittingStatsTable } from "@/components/HittingStatsTable"

type PlayerPageProps = {
    player: IndividualPlayer
}
const currentSeasonName = "Winter 2023"


export const PlayerPage = ({ player }: PlayerPageProps) => {

    const currentSeasonStats = player.SeasonTotals.find(s => s.SeasonName === currentSeasonName)

    return (
        <div>
            <div className="bg-gray-800 text-sm text-white rounded-md p-4 dark:bg-white dark:text-gray-800 mb-5" role="alert">
                <span className="font-bold">WARNING!</span> This page is a work in progress. The stats on this page may be incorrect, accurate stats see the individual game pages.
            </div>
            <h1 className="text-2xl mb-5">{player.Name}</h1>

            <div>
                <h2 className="text-xl mb-2">Current Season</h2>
                <div className="border-b border-gray-200 dark:border-gray-700">
                    <nav className="flex space-x-2" aria-label="Tabs" role="tablist">
                        <button type="button"
                            className="hs-tab-active:font-semibold hs-tab-active:border-blue-600 hs-tab-active:dark:border-grey-300 py-4 px-1 inline-flex items-center gap-2 border-b-[3px] border-transparent 
                        text-sm whitespace-nowrap text-gray-500 hover:text-blue-600 active" id="tabs-with-underline-item-1" data-hs-tab="#tabs-with-underline-1" aria-controls="tabs-with-underline-1" role="tab">
                            Hitting
                        </button>
                        <button type="button" className="hs-tab-active:font-semibold hs-tab-active:border-blue-600 hs-tab-active:dark:border-grey-300 py-4 px-1 inline-flex items-center gap-2 border-b-[3px] border-transparent text-sm whitespace-nowrap text-gray-500 hover:text-blue-600" id="tabs-with-underline-item-2" data-hs-tab="#tabs-with-underline-2" aria-controls="tabs-with-underline-2" role="tab">
                            Fielding
                        </button>
                        <button type="button"
                            className="hs-tab-active:font-semibold hs-tab-active:border-blue-600 hs-tab-active:dark:border-grey-300 py-4 px-1 inline-flex items-center gap-2 border-b-[3px] border-transparent text-sm whitespace-nowrap text-gray-500 hover:text-blue-600" id="tabs-with-underline-item-3" data-hs-tab="#tabs-with-underline-3" aria-controls="tabs-with-underline-3" role="tab">
                            Pitching
                        </button>
                    </nav>
                </div>

                <div className="mt-3">
                    <div id="tabs-with-underline-1" role="tabpanel" aria-labelledby="tabs-with-underline-item-1">
                        <HittingStatsTable players={[{ ...(currentSeasonStats! as any as Player) }]} />

                    </div>
                    <div id="tabs-with-underline-2" className="hidden" role="tabpanel" aria-labelledby="tabs-with-underline-item-2">
                        <p className="text-gray-500 dark:text-gray-400">
                            You're an eager beaver, aren't you? Well so am I... <em className="font-semibold text-gray-800 dark:text-gray-200">Fielding</em> stats are coming right up!
                        </p>
                    </div>
                    <div id="tabs-with-underline-3" className="hidden" role="tabpanel" aria-labelledby="tabs-with-underline-item-3">
                        <p className="text-gray-500 dark:text-gray-400">
                            You're an eager beaver, aren't you? Well so am I... <em className="font-semibold text-gray-800 dark:text-gray-200">Pitching</em> stats are coming right up!
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default PlayerPage



export const getStaticProps: GetStaticProps<PlayerPageProps, { playerShortId?: string }> = async ({ params }) => {

    const gameShortId = params?.["playerShortId"]
    console.log("getStaticProps", { shortId: gameShortId })

    const content = fs.readFileSync(process.cwd() + `/data/player-output/${gameShortId}.json`).toString()

    const player = JSON.parse(content) as IndividualPlayer || []

    return {
        props: {
            player: player || {},
        },
    }
}

export const getStaticPaths = async () => {
    const content = fs.readFileSync(process.cwd() + "/data/players.json").toString()

    const players = JSON.parse(content) as PlayerShortIds || []

    const paths = players
        .map((playerShortId) => ({ params: { playerShortId: playerShortId } }))

    return {
        paths: paths,
        fallback: false,
    }
}




